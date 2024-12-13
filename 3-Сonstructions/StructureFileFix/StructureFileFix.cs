using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSDK;

namespace masshtab
{
    [Transaction(TransactionMode.Manual)]
    public class StructureFileFix : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                string className = "Ускорить файл КЖ"; DateTime dateTime = DateTime.Now;
            if (RevitApi.UiApplication == null) { RevitApi.Initialize(commandData); }
            UIDocument uidoc = RevitApi.UiDocument; Document doc = RevitApi.Document;
            UIApplication uiApp = RevitApi.UiApplication; Autodesk.Revit.ApplicationServices.Application rvtApp = uiApp.Application;
            // создание log - файла
            new Logger(dateTime, className, "Старт работы;");
                 
            new Logger(dateTime, className, "Сбор элементов;"); 
            //получаем все типы арматуры
            List<RebarBarType> rebarTypes = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .OfClass(typeof(RebarBarType))
                .Cast<RebarBarType>()
                .ToList();
            new Logger(dateTime, className, "Найдены типы арматуры в кол-ве " + rebarTypes.Count.ToString() + " шт;");

            if (rebarTypes.Count == 0)
            {
                    new S_Mistake_String("Ошибка. В данной модели отсутствуют типы арматурных стержней!").ShowDialog();
                    //new iWindow("В данной модели отсутствуют типы арматурных стержней!").ShowDialog();
                return Result.Failed;
            }

            new Logger(dateTime, className, "Собираем общие параметры проекта, добавленные для арматуры по типу");
            Dictionary<string, Param> projectParamsStorage = new Dictionary<string, Param>();
            RebarBarType firstBarType = rebarTypes.First();
            foreach (Parameter param in firstBarType.ParametersMap)
            {
                string paramName = param.Definition.Name;
                if (!param.IsShared) continue;
                Param mpsp = new Param(param, doc);
                projectParamsStorage.Add(paramName, mpsp);
                new Logger(dateTime, className, "Общий параметр найден: " + paramName);
            }

            new Logger(dateTime, className, "Запоминаем типы арматуры;");
            //запоминаем все типы арматуры со значениями параметров
            List<Rebar> myrebarTypes = new List<Rebar>();

            foreach (RebarBarType rbt in rebarTypes)
            {
                Rebar mrt = new Rebar(rbt);
                myrebarTypes.Add(mrt);
                new Logger(dateTime, className, "Тип сохранен: " + mrt.bartype.Name);
            }

            new Logger(dateTime, className, "ФОП;");
            DefinitionFile deffile = null;
            try
            {
                deffile = commandData.Application.Application.OpenSharedParameterFile();
            }
            catch
            {
                    //new iWindow("Не найден файл общих параметров!").ShowDialog();
                    new S_Mistake_String("Ошибка. Не найден файл общих параметров!").ShowDialog();
                    string commandText = @"https://docs.google.com/document/d/1jajthNsi7Og4Uir7st2rqkeHWUoaNgLOpSToB1odFQc/edit#bookmark=id.6rptroc6lrv7";
                var proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = commandText;
                proc.StartInfo.UseShellExecute = true;
                proc.Start();
                new Logger(dateTime, className, "Не найден файл общих параметров.");
                return Result.Cancelled;
            }

            if (deffile == null)
            {
                    new S_Mistake_String("Ошибка. Некорректный файл общих параметров!").ShowDialog();
                    //new iWindow("Некорректный файл общих параметров!").ShowDialog();
                new Logger(dateTime, className, "Некорректный файл общих параметров.");
                return Result.Cancelled;
            }

            //удаляем параметр проекта (если только 1 категория) или снимаем флажок с категории несущей арматуры (если категорий несколько)
            new Logger(dateTime, className, "Чистим параметры арматуры в проекте;");
            using (Transaction t = new Transaction(doc))
            {
                new Logger(dateTime, className, "Открываем транзакцию 1;");
                t.Start("Ускорить файл КЖ (1 этап)");
                {
                    foreach (var kvp in projectParamsStorage)
                    {
                        Param myProjectParam = kvp.Value;
                        if (myProjectParam.categories.Count == 1)
                        {
                            //параметр только для несущей арматуры, значит надо удалить целиком
                            //перед этим проверяем, есть ли параметр в ФОП

                            bool checkParamExistsInDefFile = ParamTools.CheckParameterExistsInFile(deffile, myProjectParam.guid);
                            if (!checkParamExistsInDefFile)
                            {
                                ParamTools.AddParameterToDefFile(deffile, "NonTemplate parameters", myProjectParam);
                            }


                            doc.ParameterBindings.Remove(myProjectParam.def);
                            new Logger(dateTime, className, "   Удален: " + myProjectParam.Name);
                        }
                        else
                        {
                            //категорий несколько, надо убрать флажок с категории несущей арматуры
                            myProjectParam.RemoveOrAddFromRebarCategory(doc, firstBarType, false);
                            new Logger(dateTime, className, "   Снят флажок с несущей арматуры: " + myProjectParam.Name);
                        }
                    }
                }
                t.Commit();
                new Logger(dateTime, className, "Закрываем транзакцию 1;");
            }

            new Logger(dateTime, className, "Параметры удалены, возвращаем обратно;");

            //возвращаем параметры обратно
            using (Transaction t2 = new Transaction(doc))
            {
                t2.Start("Ускорить файл КЖ (2 этап)");
                new Logger(dateTime, className, "Открываем транзакцию 2;");

                foreach (var kvp in projectParamsStorage)
                {
                    Param myProjectParam = kvp.Value;
                    if (myProjectParam.categories.Count == 1)
                    {
                        //параметр был назначен только несущей арматуре, был удален совсем, значит создаем параметр
                        myProjectParam.AddToProjectParameters(doc, firstBarType);
                        new Logger(dateTime, className, "   Добавлен: " + myProjectParam.Name);
                    }
                    else
                    {
                        //категорий было несколько, возвращаем флажок к категории несущей арматуры
                        myProjectParam.RemoveOrAddFromRebarCategory(doc, firstBarType, true);
                        new Logger(dateTime, className, "   Добавлен флажок для несущей арматуры: " + myProjectParam.Name);
                    }
                }

                t2.Commit();
                new Logger(dateTime, className, "Закрываем транзакцию 2;");
            }

            new Logger(dateTime, className, "Восстанавливаем значения параметров;");
            //восстанавливаем значения у типов арматуры
            using (Transaction t3 = new Transaction(doc))
            {
                t3.Start("Ускорить файл КЖ (3 этап)");
                new Logger(dateTime, className, "Открываем транзакцию 3;");

                foreach (Rebar mrt in myrebarTypes)
                {
                    RebarBarType rbt = mrt.bartype;
                    new Logger(dateTime, className, "Тип: " + mrt.Name);

                    foreach (Parameter param in rbt.ParametersMap)
                    {
                        string paramName = param.Definition.Name;
                        ParamValue mpv = mrt.ValuesStorage[paramName];
                        if (mpv.IsNull) continue;
                        mpv.SetValue(param);
                        new Logger(dateTime, className, "   Параметр: " + paramName + ", значение " + mpv.ToString());
                    }
                }

                t3.Commit();
                new Logger(dateTime, className, "Закрываем транзакцию 3;");
            }
            //string endTime = DateTime.Now.ToLongTimeString();
            //string msg = "Выполнено! Время старта: " + startTime + ", окончания: " + endTime;
            new S_Mistake_String("Успешно! Файл станет быстрее.").ShowDialog();
                //new iWindow("Успешно! Файл станет быстрее.").ShowDialog();
            //Debug.WriteLine(msg);

            new Logger(dateTime, className, "Завершение работы.");
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
            return Result.Succeeded;
           
        }
    }
}

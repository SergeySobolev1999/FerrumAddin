using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace masshtab
{
    [Transaction(TransactionMode.Manual)]
    public class PinnerWorksets : IExternalCommand
    {
        private void WorksetCheck(in List<Workset> worksets0, in RevitLinkInstance link, out bool wsExists)
        {
            wsExists = false;
            WorksetId lwid = link.WorksetId; //получаем id набора связи
            string lname = link.Name;
            string[] nameparts = lname.Split(new char[] { ':' });
            lname = nameparts[0];
            lname = lname.Replace(".rvt ", "");
            foreach (var workset in worksets0)
            {
                if (workset.Name.Contains(lname))
                {
                    wsExists = true; break;
                }
            }
        }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            string className = "Закрепление и наборы"; DateTime dateTime = DateTime.Now;
            if (RevitApi.UiApplication == null) { RevitApi.Initialize(commandData); }
            //создание log-файла
            new Logger(dateTime, className, "Старт работы;");

            Document doc = RevitApi.Document;
            int failscount1 = 0;

            List<Autodesk.Revit.DB.Grid> grids = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Grids) //Оси
                                                                                     .WhereElementIsNotElementType()
                                                                                     .Cast<Autodesk.Revit.DB.Grid>()
                                                                                     .ToList();

            List<Level> levels = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Levels) //Уровни
                                                                         .WhereElementIsNotElementType()
                                                                         .Cast<Level>()
                                                                         .ToList();

            List<RevitLinkInstance> links = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_RvtLinks) //Связи
                                                                         .WhereElementIsNotElementType()
                                                                         .Cast<RevitLinkInstance>()
                                                                         .ToList();

            List<Workset> worksets0 = new FilteredWorksetCollector(doc) //рабочие наборы документа
                .OfKind(WorksetKind.UserWorkset)
                .Cast<Workset>()
                .ToList();

            new Logger(dateTime, className, "Элементы собраны. Создаем списки для работы, проверяем, является ли модель файлом хранилища;");

            List<string> linkNames = new List<string>(); //пустой список имен связей
            List<string> linkNamesToCreateWorksets = new List<string>(); //пустой список имен связей для создания наборов
            List<string> failed1 = new List<string>(); //пустой список id связей с недоступным параметром Рабочий набор
            List<RevitLinkInstance> linksToChange = new List<RevitLinkInstance>(); //пустой список изменяемых связей

            bool dws = doc.IsWorkshared;
            if (dws) { new Logger(dateTime, className, "Модель является файлом хранилища;"); }
            else { new Logger(dateTime, className, "Модель НЕ является файлом хранилища;"); }

            bool deleteWorksets = false;
            if (dws)
            {
                wsViewModel viewModel = new wsViewModel(); //вьюмодель
                ///тут можно вставить десериализацию вьюмодели
                wswpf wpfView = new wswpf(viewModel); //окно настроек запуска
                viewModel.CloseRequest += (s, e) => wpfView.Close();
                bool? ok = wpfView.ShowDialog();
                if (ok != null && ok == true) { } else { return Result.Cancelled; }
                deleteWorksets = viewModel.delete;
                ///тут можно сериализовать вьюмодель
            }

            if (dws&&links.Count>0)
            {
                new Logger(dateTime, className, "Заполняем список связей на изменение;");
                foreach (var link in links) 
                {
                    string lname = link.Name; new Logger(dateTime, className, link.Name);
                    string[] nameparts = lname.Split(new char[] { ':' }); 
                    lname = nameparts[0];
                    lname = lname.Replace(".rvt ", ""); //получаем имя связанного файла
                    linkNames.Add(lname); //добавляем его в список имен связей
                    WorksetCheck(worksets0, link, out bool wsExists); //проверяем наличие набора с тем же именем
                    linksToChange.Add(link); //добавляем связь в список связей на изменение
                    if (!wsExists) { linkNamesToCreateWorksets.Add(lname); new Logger(dateTime, className, "   требуется создать набор для связи"); }
                }
                new Logger(dateTime, className, "Список изменяемых связей:");
                foreach (var l in linksToChange) { new Logger(dateTime, className, "   " + l.Name + ";"); }
            }

            //Транзакция           

            using (Transaction t = new Transaction(doc))
            {

                t.Start("Закрепление и наборы");
                new Logger(dateTime, className, "Открываем транзакцию;");

                foreach (var grid in grids)
                {
                    string eid = grid.Id.ToString();
                    try
                    {
                        grid.Pinned = true; new Logger(dateTime, className, "   ось " + eid + ": закреплена;");
                    }
                    catch (Exception ex) { new Logger(dateTime, className, "   ось " + eid + ": Ошибка: " + ex.Message); continue; }
                }

                new Logger(dateTime, className, "Закреплятор - уровни:");
                foreach (var level in levels)
                {
                    string eid = level.Id.ToString();
                    try
                    {
                        level.Pinned = true; new Logger(dateTime, className, "   уровень " + eid + ": закреплен;");
                    }
                    catch (Exception ex) { new Logger(dateTime, className, "   уровень " + eid + ": Ошибка: " + ex.Message); continue; }
                }

                new Logger(dateTime, className, "Закреплятор - связи:");
                foreach (var link in links)
                {
                    string eid = link.Id.ToString();
                    try
                    {
                        link.Pinned = true; new Logger(dateTime, className, "   связь " + eid + ": закреплена;");
                    }
                    catch (Exception ex) { new Logger(dateTime, className, "   связь " + eid + ": Ошибка: " + ex.Message); continue; }
                }

                if (dws)
                {
                    if (links.Count > 0)
                    {
                        new Logger(dateTime, className, "Создание наборов;");
                        foreach (string name in linkNamesToCreateWorksets)
                        {
                            try
                            {
                                Workset ws = Workset.Create(doc, name); //создаем наборы для связей
                                new Logger(dateTime, className, "   Создаем набор " + name + ";");
                            }
                            catch (Exception ex) { failed1.Add(name); failscount1++; new Logger(dateTime, className, "      Ошибка: " + ex.Message); continue; }
                        }
                    }


                    new Logger(dateTime, className, "Обновление списка наборов;");
                    List<Workset> worksets1 = new FilteredWorksetCollector(doc) //рабочие наборы документа
                                         .OfKind(WorksetKind.UserWorkset)
                                         .Cast<Workset>()
                                         .ToList();
                    List<int> widsGL = new List<int>(); //пустой список номеров РН осей/уровней
                    

                    if (links.Count > 0)
                    {
                        new Logger(dateTime, className, "Назначение наборов связям;");
                        foreach (var workset in worksets1)
                        {
                            string wname = workset.Name; new Logger(dateTime, className, "   Набор " + workset.Name + ";");
                            if (wname == "Оси и уровни" || wname == "Общие слои и сетки" || wname == "Общие уровни и сетки")
                            {
                                widsGL.Add(workset.Id.IntegerValue); 
                            }
                            foreach (var link in linksToChange)
                            {
                                string lname = link.Name;
                                string[] nameparts = lname.Split(new char[] { ':' });
                                lname = nameparts[0];
                                lname = lname.Replace(".rvt ", "");
                                Autodesk.Revit.DB.Parameter param = link.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM);//получаем параметр "РН"
                                if (workset.Name.Contains(lname))
                                {
                                    try
                                    {
                                        param.Set(workset.Id.IntegerValue); //назначаем РН
                                        new Logger(dateTime, className, "      Назначаем набор " + lname + ";");
                                    }
                                    catch (Exception ex) { failed1.Add(link.Id.ToString()); failscount1++; new Logger(dateTime, className, "      Ошибка: " + ex.Message); continue; }
                                    break;
                                }
                            }
                        }
                    }

                    //Назначаем набор осям уровням
                    new Logger(dateTime, className, "Назначаем набор осям;");
                    foreach (var grid in grids)
                    {
                        Autodesk.Revit.DB.Parameter param = grid.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM);//получаем параметр "РН"
                        //new Logger(dateTime, className, "   Ось " + grid.Id + ";");
                        if (widsGL.Count > 0) param.Set(widsGL[0]); //берем первое значение из списка номеров РН осей/уровней
                    }
                    new Logger(dateTime, className, "Назначаем набор уровням;");
                    foreach (var level in levels)
                    {
                        Autodesk.Revit.DB.Parameter param = level.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM);//получаем параметр "РН"
                        //new Logger(dateTime, className, "   Уровень " + level.Id + ";");
                        if (widsGL.Count > 0) param.Set(widsGL[0]); //берем первое значение из списка номеров РН осей/уровней
                    }
                }

                t.Commit();
                new Logger(dateTime, className, "Закрываем транзакцию;");

            }

            //Удаляем лишние наборы, содержащие в имени первые 3 символа шифра и не назначенные связям
            if (deleteWorksets && links.Count > 0)
            {
                List<Workset> worksets2 = new FilteredWorksetCollector(doc) //рабочие наборы документа
                                         .OfKind(WorksetKind.UserWorkset)
                                         .Cast<Workset>()
                                         .ToList();
                List<Workset> worksetsGL = new List<Workset>();
                foreach(var workset in worksets2)
                {
                    string wname = workset.Name; 
                    if (wname == "Оси и уровни" || wname == "Общие слои и сетки" || wname == "Общие уровни и сетки") worksetsGL.Add(workset);
                }

                

                new Logger(dateTime, className, "Ищем наборы для удаления;");
                string firstLinkName = linkNames[0]; string projectcode = firstLinkName.Substring(0, 2);
                new Logger(dateTime, className, "Текст для поиска в имени связи: " + projectcode + ";");

                List<Workset> worksetsToRemove = new List<Workset>();

                foreach (var ws in worksets2)
                {
                    new Logger(dateTime, className, "   Набор " + ws.Name + ";");
                    int countTrue = 0;
                    foreach (var link in linksToChange)
                    {
                        string lname = link.Name;
                        string[] nameparts = lname.Split(new char[] { ':' });
                        lname = nameparts[0];
                        lname = lname.Replace(".rvt ", "");
                        if (ws.Name.Contains(projectcode) && ws.Name.Contains(lname))
                        {
                            WorksetId linkWorksetId = link.WorksetId;
                            if (linkWorksetId == ws.Id) countTrue++;
                        }
                    }
                    if (countTrue == 0&&ws.Name.Contains(projectcode)) { worksetsToRemove.Add(ws); new Logger(dateTime, className, "      удалить;"); }

                }
                if(worksetsToRemove.Count > 0)
                {
                    using (Transaction t2 = new Transaction(doc))
                    {

                        t2.Start("Поиск наборов для удаления");
                        new Logger(dateTime, className, "Открываем транзакцию;");

                        int i = 0;
                        foreach (Workset ws in worksetsToRemove)
                        {
                            string datetimenow = DateTime.Now.ToString();
                            datetimenow = datetimenow.Replace("/", "");
                            datetimenow = datetimenow.Replace(".", "");
                            datetimenow = datetimenow.Replace(":", "");
                            WorksetTable.RenameWorkset(doc, ws.Id, "удалить" + i.ToString() + " " + datetimenow);
                            DeleteWorksetOption deleteWorksetOption = DeleteWorksetOption.MoveElementsToWorkset;
                            bool canDelete = WorksetTable.CanDeleteWorkset(doc, ws.Id, new DeleteWorksetSettings(deleteWorksetOption, worksetsGL[0].Id));
                            if (canDelete) WorksetTable.DeleteWorkset(doc, ws.Id, new DeleteWorksetSettings(deleteWorksetOption, worksetsGL[0].Id));
                            i++;
                        }

                        t2.Commit();
                        new Logger(dateTime, className, "Закрываем транзакцию;");
                    }
                    List<Workset> worksets3 = new FilteredWorksetCollector(doc) //рабочие наборы документа
                                         .OfKind(WorksetKind.UserWorkset)
                                         .Cast<Workset>()
                                         .ToList();
                    using (Transaction t3 = new Transaction(doc))
                    {

                        t3.Start("Удаление наборов");
                        new Logger(dateTime, className, "Открываем транзакцию;");

                        foreach (Workset ws in worksets3)
                        {
                            if (ws.Name.Contains("удалить"))
                            {
                                DeleteWorksetOption deleteWorksetOption = DeleteWorksetOption.MoveElementsToWorkset;
                                bool canDelete = WorksetTable.CanDeleteWorkset(doc, ws.Id, new DeleteWorksetSettings(deleteWorksetOption, worksetsGL[0].Id));
                                if (canDelete) WorksetTable.DeleteWorkset(doc, ws.Id, new DeleteWorksetSettings(deleteWorksetOption, worksetsGL[0].Id));
                            }
                        }

                        t3.Commit();
                        new Logger(dateTime, className, "Закрываем транзакцию;");
                    }
                }
                
            }
            new iWindow("Готово!").ShowDialog();
            return Result.Succeeded;
        }
    }
}

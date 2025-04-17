using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApplication.Licenses;
using Autodesk.Revit.ApplicationServices;

namespace WPFApplication.MainRemovingOpenings
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class MainRemovingOpenings : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                Document document = commandData.Application.ActiveUIDocument.Document;
                int numUngroupMembers = 0;
                int numRectOpening = 0;
                int numUnAssemblingMembers = 0;
                int numToZeroParamters = 0;
                int numToZeroElements = 0;
                SSDK_Data.licenses_Name = Environment.UserName;
                if (SSDK_Data.licenses_Connection)
                {
                    if (SSDK_Data.licenses_Post == "Конструктор расчетчик")
                    {

                        FilteredElementCollector collectorsGroups = new FilteredElementCollector(document);
                    ICollection<Group> all_Elements_Group = collectorsGroups.OfCategory(BuiltInCategory.OST_IOSModelGroups).WhereElementIsNotElementType().ToElements().Select(a=>a as Group).ToList();
                    using (Transaction transactionGroup = new Transaction(document, "Разгруппировка групп модели"))
                    {
                        if (transactionGroup.Start() == TransactionStatus.Started)
                        {
                        //FailureHandlingOptions failureOptions = transactionGroup.GetFailureHandlingOptions();
                        //failureOptions.SetFailuresPreprocessor(new IgnoreWarningPreprocessor());
                        //failureOptions.SetForcedModalHandling(false);  // Отключаем диалоговые окна
                        //failureOptions.SetClearAfterRollback(true);    // Очищаем после отката
                        //failureOptions.SetDelayedMiniWarnings(false);  // Отключаем мини-предупреждения
                        //transactionGroup.SetFailureHandlingOptions(failureOptions);
                        foreach (Group group in all_Elements_Group)
                            {
                                group.UngroupMembers();
                                document.Delete(group.Id);
                                numUngroupMembers++;
                            }
                        }
                    transactionGroup.Commit();
                    }

                    FilteredElementCollector collectorsAssemblings = new FilteredElementCollector(document);
                    ICollection<AssemblyInstance> all_Elements_AssemblyInstances = collectorsAssemblings.OfCategory(BuiltInCategory.OST_Assemblies).WhereElementIsNotElementType().ToElements().Select(a => a as AssemblyInstance).ToList();
                    using (Transaction transactionGroup = new Transaction(document, "Разгруппировка групп модели"))
                    {
                        if (transactionGroup.Start() == TransactionStatus.Started)
                        {
                        //FailureHandlingOptions failureOptions = transactionGroup.GetFailureHandlingOptions();
                        //failureOptions.SetFailuresPreprocessor(new IgnoreWarningPreprocessor());
                        //failureOptions.SetForcedModalHandling(false);  // Отключаем диалоговые окна
                        //failureOptions.SetClearAfterRollback(true);    // Очищаем после отката
                        //failureOptions.SetDelayedMiniWarnings(false);  // Отключаем мини-предупреждения
                        //transactionGroup.SetFailureHandlingOptions(failureOptions);
                        foreach (AssemblyInstance assemblyInstance in all_Elements_AssemblyInstances)
                            {
                            assemblyInstance.Disassemble();
                            numUnAssemblingMembers++;
                            }
                        }
                        transactionGroup.Commit();
                    }
                    FilteredElementCollector collectorsRectOpening = new FilteredElementCollector(document);
                    ICollection<Element> all_Elements_RectOpening = collectorsRectOpening.OfCategory(BuiltInCategory.OST_SWallRectOpening).WhereElementIsNotElementType().ToElements().ToList();
                    using (Transaction transactionGroup = new Transaction(document, "Разгруппировка групп модели"))
                    {
                        if (transactionGroup.Start() == TransactionStatus.Started)
                        {
                            //FailureHandlingOptions failureOptions = transactionGroup.GetFailureHandlingOptions();
                            //failureOptions.SetFailuresPreprocessor(new IgnoreWarningPreprocessor());
                            //failureOptions.SetForcedModalHandling(false);  // Отключаем диалоговые окна
                            //failureOptions.SetClearAfterRollback(true);    // Очищаем после отката
                            //failureOptions.SetDelayedMiniWarnings(false);  // Отключаем мини-предупреждения
                            //transactionGroup.SetFailureHandlingOptions(failureOptions);
                            foreach (Element group in all_Elements_RectOpening)
                            {
                                document.Delete(group.Id);
                                numRectOpening++;
                            }
                        }
                    transactionGroup.Commit();
  
                    }
                        //document.Regenerate();
                        FilteredElementCollector collectorDoors = new FilteredElementCollector(document);
                    FilteredElementCollector collectorWindows = new FilteredElementCollector(document);
                    ICollection<Element> all_DoorsWindonws = collectorDoors.OfCategory(BuiltInCategory.OST_Doors).WhereElementIsNotElementType().ToElements();
                    all_DoorsWindonws = all_DoorsWindonws.Concat(collectorWindows.OfCategory(BuiltInCategory.OST_Windows).WhereElementIsNotElementType().ToElements()).ToList();
                    double sizeFront = 10 / 304.8;
                    Dictionary<string,double> dictionatySize = new Dictionary<string,double>
                    {
                         { "БЭР_Привязка_К_Наружной_Отделке",sizeFront },
                         { "Привязка к фасаду",sizeFront },
                         { "Привязка к отделочному слою спереди",sizeFront },

                         { "Вставка окна",0 },
                         { "Вставка(Четверть.Глубина)",0 },
                         { "БЭР_Вставка",0 },
                    };
                    using (Transaction transactionElements = new Transaction(document, "Обнуление параметров проемов"))
                    {
                        if (transactionElements.Start() == TransactionStatus.Started)
                        {
                        //FailureHandlingOptions failureOptions = transactionElements.GetFailureHandlingOptions();
                        //failureOptions.SetFailuresPreprocessor(new IgnoreWarningPreprocessor());
                        //failureOptions.SetForcedModalHandling(false);  // Отключаем диалоговые окна
                        //failureOptions.SetClearAfterRollback(true);    // Очищаем после отката
                        //failureOptions.SetDelayedMiniWarnings(false);  // Отключаем мини-предупреждения
                        //transactionElements.SetFailureHandlingOptions(failureOptions);
                        foreach (Element element in all_DoorsWindonws)
                            {
                                foreach(var position in dictionatySize)
                                {
                                    Parameter parameter = element.LookupParameter(position.Key);
                                    if (parameter != null && !parameter.IsReadOnly)
                                    {
                                        parameter.Set(position.Value);
                                        numToZeroParamters++;
                                    }
                                }
                                numToZeroElements++;
                            }
                        }
                    transactionElements.Commit();
                    }
                    S_Mistake_String s_Mistake_String = new S_Mistake_String($"Выполнено:\n" +
                        $"Разгруппированных групп моделей - {numUngroupMembers}\n" +
                        $"Разобранных сборок - {numUnAssemblingMembers}\n" +
                        $"Удаленных системных проемов - {numRectOpening}\n" +
                        $"Обработанных элементов - {numToZeroElements}\n" +
                        $"Обработанных параметров - {numToZeroParamters}");
                    s_Mistake_String.ShowDialog();
                    }
                    else
                    {
                        S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. Вы не являетесь расчетчиком. Плагин предназначен только лишь для обработки модели перед выполнением расчетов.");
                        s_Mistake_String.ShowDialog();
                    }
                }
                else
                {
                    S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. Ваша лицензия недоступна. Выполните переподключение ");
                    s_Mistake_String.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
                return Result.Succeeded;
            }
            return Result.Succeeded;
        }
    }
    public class IgnoreWarningPreprocessor : IFailuresPreprocessor
    {
        public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
        {
            // Получаем все сообщения об ошибках
            IList<FailureMessageAccessor> failures = failuresAccessor.GetFailureMessages();

            foreach (FailureMessageAccessor failure in failures)
            {
                // Для всех типов ошибок устанавливаем разрешение
                failuresAccessor.ResolveFailure(failure);

                // Или альтернативный вариант - удаление всех ошибок
                // failuresAccessor.DeleteWarning(failure);
            }

            return FailureProcessingResult.Continue;
        }
    }
}

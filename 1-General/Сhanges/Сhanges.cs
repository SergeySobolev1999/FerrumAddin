using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApplication.Сhanges
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class Main_Сhanges : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            
            SSDK_Data.licenses_Name = Environment.UserName;
            if (SSDK_Data.licenses_Connection)
            {
                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;
                UIApplication uiApp = uidoc.Application;

                IList<Element> sheets = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_Sheets)
                    .WhereElementIsNotElementType()
                    .ToElements();

                var sheets_withoutRevision = new List<Element>();
                var sheets_withRevision = new List<Element>();
                var sheets_hasAlpha = new List<Element>();

                using (Transaction t = new Transaction(doc, "revisionAnalised"))
                {
                    t.Start();

                    IList<Element> revisionClouds = new FilteredElementCollector(doc)
                        .OfCategory(BuiltInCategory.OST_RevisionClouds)
                        .WhereElementIsNotElementType()
                        .ToElements();
                    var dct_revision_clouds = new Dictionary<string, Tuple<string, string>>();

                    if (revisionClouds.Count != 0)
                    {
                        foreach (Element revCloud in revisionClouds)
                        {
                            Parameter numParam = revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_NUM);
                            if (numParam != null)
                            {
                                string num_revCloud = revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_NUM).AsString().TrimEnd('\u202A');
                                if (dct_revision_clouds.ContainsKey(num_revCloud))
                                {
                                    continue;
                                }
                                else
                                {
                                    string date_revCloud = revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_DATE).AsString();
                                    string num_doct_revCloud = revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_ISSUED_TO).AsString();
                                    dct_revision_clouds[num_revCloud] = new Tuple<string, string>(date_revCloud, num_doct_revCloud);
                                }
                            }
                        }
                    }
                    IList<string> sorted_num_revCloud = dct_revision_clouds.Keys.OrderByDescending(k => k).Take(9).ToList();

                    foreach (Element family_instance in getTypeByName(doc, "ZH_ТаблицаРегистрацииИзменений"))
                    {
                        family_instance.LookupParameter("Ко_во изменений").Set(sorted_num_revCloud.Count + 1);
                        if (sorted_num_revCloud.Count < 9)
                        {

                            for (int i = 1; i <= 9; i++)
                            {
                                family_instance.LookupParameter($"Изм{i}").Set("");
                                family_instance.LookupParameter($"Ном.док.{i}").Set("");
                                family_instance.LookupParameter($"Дата{i}").Set("");
                            }
                            doc.Regenerate();
                        }
                        for (int i = 0; i < sorted_num_revCloud.Count; i++)
                        {
                            var numDct = sorted_num_revCloud[i];
                            var num_date_dct = dct_revision_clouds[numDct].Item1;
                            var num_doc_dct = dct_revision_clouds[numDct].Item2;

                            family_instance.LookupParameter($"Изм{i + 1}").Set(numDct);
                            family_instance.LookupParameter($"Ном.док.{i + 1}").Set(num_doc_dct);
                            family_instance.LookupParameter($"Дата{i + 1}").Set(num_date_dct);
                        }
                    }

                    foreach (ViewSheet sheet in sheets)
                    {
                        var dependent_Revision_elements = sheet.GetAllRevisionCloudIds();
                        Parameter p_Note = sheet.LookupParameter("ADSK_Примечание");

                        if (dependent_Revision_elements.Any())
                        {
                            sheets_withRevision.Add(sheet);
                            IDictionary<string, int> my_dict = new Dictionary<string, int>();
                            IDictionary<string, string> my_dict_zam_now = new Dictionary<string, string>();

                            foreach (ElementId i in dependent_Revision_elements)
                            {
                                string nameRevisionCloud = doc.GetElement(i).get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_NUM).AsString();
                                string revision_element = doc.GetElement(i).Name;

                                if (!my_dict_zam_now.ContainsKey(nameRevisionCloud.Trim()))
                                {
                                    if (revision_element.Contains("Зам."))
                                    {
                                        my_dict_zam_now[nameRevisionCloud] = "(Зам.)";
                                    }
                                    else if (revision_element.Contains("Нов."))
                                    {
                                        my_dict_zam_now[nameRevisionCloud] = "(Нов.)";
                                    }
                                    else
                                    {
                                        my_dict_zam_now[nameRevisionCloud] = "";
                                    }
                                }
                                if (revision_element.Contains("Зам.") || revision_element.Contains("Нов."))
                                {
                                    my_dict[nameRevisionCloud] = -1;
                                    continue;
                                }
                                if (my_dict.ContainsKey(nameRevisionCloud))
                                {
                                    my_dict[nameRevisionCloud]++;
                                }
                                else
                                {
                                    my_dict[nameRevisionCloud] = 1;
                                }
                            }

                            IList<string> keylist = my_dict.Keys.ToList();
                            IList<int> valuelist = my_dict.Values.ToList();

                            var zipedLst = keylist.Zip(valuelist, (k, v) => (k, v));
                            var sortedZipedLst = zipedLst.OrderBy(x => x.Item1).ToList();

                            var newLstKeys = sortedZipedLst.Select(x => x.Item1).ToList();
                            var newLstValues = sortedZipedLst.Select(x => x.Item2).ToList();

                            var filter = new ElementClassFilter(typeof(FamilyInstance));
                            var dependentElements = sheet.GetDependentElements(filter);

                            FamilyInstance nessesaryTitleBlock = null;
                            foreach (var dElement in dependentElements)
                            {
                                if (doc.GetElement(dElement).Category.BuiltInCategory == BuiltInCategory.OST_TitleBlocks)
                                {
                                    nessesaryTitleBlock = doc.GetElement(dElement) as FamilyInstance;
                                    break;
                                }
                            }
                            if (nessesaryTitleBlock != null)
                            {
                                IList<Parameter> parameters = Enumerable.Range(1, 4).Select(i => nessesaryTitleBlock.LookupParameter($"Ф3_Стр{i}_КолУч")).ToList();
                                foreach (Parameter p in parameters)
                                {
                                    p?.Set("");
                                }
                                doc.Regenerate();
                                int counter = newLstKeys.Count;
                                if (counter < 5)
                                {
                                    for (int i = 0; i < newLstValues.Count; i++)
                                    {
                                        string valueToWrite = "-";
                                        if (newLstValues[i] > 0)
                                        {
                                            valueToWrite = newLstValues[i].ToString();
                                        }
                                        parameters[i].Set(valueToWrite);
                                    }
                                }
                                else
                                {
                                    newLstValues = newLstValues.Skip(newLstValues.Count - 4).ToList();
                                    for (int i = 0; i < newLstValues.Count; i++)
                                    {
                                        if (parameters[i] != null)
                                        {
                                            parameters[i].Set(newLstValues[i].ToString());
                                        }
                                    }
                                }
                                if (p_Note != null)
                                {
                                    var keys_lst_zam_not = my_dict_zam_now.Keys;
                                    var values_lst_zam_not = my_dict_zam_now.Values;
                                    var zipped_lst_zam_now = keys_lst_zam_not.Zip(values_lst_zam_not, (k, v) => (k, v));
                                    var sorted_ziped_lst_zam_now = zipped_lst_zam_now.OrderBy(x => x.Item1).ToList();

                                    var itogTxt = sorted_ziped_lst_zam_now.Select(x => $"{x.Item1}{x.Item2}").ToList();
                                    var txt = $"Изм.{string.Join(";", itogTxt)}";
                                    p_Note.Set(txt);
                                }
                            }
                        }
                        else
                        {
                            sheets_withoutRevision.Add(sheet);
                            var filter = new ElementClassFilter(typeof(FamilyInstance));
                            var dependentElements = sheet.GetDependentElements(filter);

                            FamilyInstance necessaryTitleBLock = null;
                            foreach (ElementId dElement in dependentElements)
                            {
                                if (doc.GetElement(dElement).Category.BuiltInCategory == BuiltInCategory.OST_TitleBlocks)
                                {
                                    necessaryTitleBLock = doc.GetElement(dElement) as FamilyInstance;
                                    break;
                                }
                            }
                            if (necessaryTitleBLock != null)
                            {
                                var parameters = Enumerable.Range(1, 4).Select(i => necessaryTitleBLock.LookupParameter($"Ф3_Стр{i}_КолУч")).ToList();
                                foreach (Parameter p in parameters)
                                {
                                    p?.Set("");
                                }
                                if (p_Note != null)
                                {
                                    p_Note.Set("");
                                }
                            }
                        }

                    }
                    t.Commit();
                }
            S_Mistake_String s_Mistake_String = new S_Mistake_String($"Процесс прошел успешно!\nЛистов с изменениями: { sheets_withRevision.Count }\n" +
                    $"Листов без изменений: {sheets_withoutRevision.Count}");
            s_Mistake_String.ShowDialog();
            }
            else
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. Ваша лицензия недоступна. Выполните переподключение");
                s_Mistake_String.ShowDialog();
            }
            return Result.Succeeded;
        }
        private IEnumerable<Element> getTypeByName(Document doc, string typeName)
        {
            ElementId paramType = new ElementId(BuiltInParameter.ELEM_TYPE_PARAM);
            ParameterValueProvider fParam = new ParameterValueProvider(paramType);
            FilterStringEquals evaluator = new FilterStringEquals();
            FilterStringRule fRule = new FilterStringRule(fParam, evaluator, typeName);
            ElementParameterFilter filter_type_name = new ElementParameterFilter(fRule);
            return new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_GenericAnnotation)
                .WherePasses(filter_type_name)
                .WhereElementIsNotElementType()
                .ToElements();
        }
    }
}

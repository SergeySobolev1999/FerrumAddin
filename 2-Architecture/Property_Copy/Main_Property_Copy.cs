using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using System.Text;
using System.Threading.Tasks;
using WPFApplication.Assembling_Project_On_Group_Stained_Glass_Windows;
using System.Xml.Linq;
using System.IO;

namespace WPFApplication.Property_Copy
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class Main_Class_Property_Copy : IExternalCommand
    {

        private static ExternalEvent _externalEvent;
        private static PropertyCopyHandler _handler = new PropertyCopyHandler();

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            if (1 == 2)
            {
                UIApplication uiApp = commandData.Application;
            Application application = uiApp.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document_Property_Copy_Donor.Document = uidoc.Document;
            Document_Property_Copy_Target.Document = uidoc.Document;
            SSDK_Data.username = Environment.UserName;
            if (1==1)
            {
                //UIApplication uiApp = commandData.Application;
                //Application application = uiApp.Application;
                //Document doc = uidoc.Document;
               
                //Создаем ExternalEvent(если не создан)
                if (_externalEvent == null)
                {
                    _externalEvent = ExternalEvent.Create(_handler);
                }

                WPF_Main_Property_Copy wPF_Main_Property_Copy = new WPF_Main_Property_Copy(commandData);

                wPF_Main_Property_Copy.Closed += (sender, e) =>
                {
                    // Передаем данные в обработчик
                    List<Parameter_Identification> convertedParameters =
                    Data_Class_Property_Copy.parameters.Cast<Parameter_Identification>().ToList();
                    _handler.SetData(Data_Class_Property_Copy.elements_Target_Elements, convertedParameters, Document_Property_Copy_Donor.Document, Document_Property_Copy_Target.Document);
                    _externalEvent.Raise(); // Запускаем обновление параметров
                };

                wPF_Main_Property_Copy.Show(); // Немодальное окно
            }
            else
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. Плагин в разработке");
                s_Mistake_String.ShowDialog();
            }
            }
            else
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. Плагин в разработке");
                s_Mistake_String.ShowDialog();
            }
            return Result.Succeeded;
        }
    }
    public class PropertyCopyHandler : IExternalEventHandler
    {
        private List<Element> elements;
        private List<Parameter_Identification> parameters;
        private Document document_Donor { get; set; }
        private Document document_Target { get; set; }
        public void Execute(UIApplication uiApp)
        {
            //foreach (Element element1 in elements)
            //{
            //    try
            //    {

            //        foreach (Parameter_Identification parameter_Position in parameters)
            //        {
            //            //parameter_Position.parameter;
                    
                   
            //        //List<FamilySymbol> window = new FilteredElementCollector(doc_Family).OfClass(typeof(FamilySymbol)).Cast<FamilySymbol>().Where(fs => fs.Family.Name == family.Id).ToList();
            //        }

            //    }

            //    catch (Exception ex)
            //    {
            //        S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
            //        s_Mistake_String.ShowDialog();
            //    }
            //}
            List<Element> collection_Type_Ghost_Target = new FilteredElementCollector(document_Target).OfCategory(BuiltInCategory.OST_GenericAnnotation).WhereElementIsElementType().ToList();
            Family loadedFamily;
            foreach (Element element in elements)
            {
                foreach (Parameter_Identification parameter_Position in parameters)
                {
                    Element elementType = document_Target.GetElement(element.GetTypeId());
                    string stoc_Designation = "";
                    string stoc_Type_Name = "";
                    Parameter param = elementType.LookupParameter(parameter_Position.parameter.Definition.Name);
                    try
                    {
                        if (param.StorageType == StorageType.ElementId && param.Definition.GetDataType().TypeId == "autodesk.revit.category.family:genericAnnotation-1.0.0")
                        {
                            FamilySymbol familySymbol = document_Target.GetElement(element.GetTypeId()) as FamilySymbol;
                            Family family = familySymbol.Family;
                            Document doc_Family = document_Target.EditFamily(family);
                            string name = param.AsValueString();
                            string[] stoc_Designation_Perview = name.Split(new[] { " : " }, StringSplitOptions.None);
                            stoc_Designation = stoc_Designation_Perview[0];
                            stoc_Type_Name = stoc_Designation_Perview[stoc_Designation_Perview.Count() - 1];
                            //List<FamilySymbol> collection_Type = new FilteredElementCollector(document_Target).OfClass(typeof(FamilySymbol)).Cast(FamilySymbol).Where(fs => stoc_Designation.Contains(fs.Family.Name));
                            Element elem = doc_Family.GetElement(element.GetTypeId());
                            //string a = parameter_Position.parameter.AsValueString();
                            //string name = document_Donor.GetElement(parameter_Position.parameter.AsElementId()).Name;
                            //List<ElementId> window = new FilteredElementCollector(doc_Family).OfClass(typeof(FamilySymbol)).Cast<FamilySymbol>().Where(fs => fs.Family.Name == document_Donor.GetElement(parameter_Position.parameter.AsElementId()).Id).ToList();
                            string tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), family.Name + ".rfa");
                            Document doc = uiApp.ActiveUIDocument.Document;
                            Family annotationFamily = new FilteredElementCollector(doc_Family)
                                                        .OfClass(typeof(Family))
                                                        .Cast<Family>()
                                                        .Where(f => f.FamilyCategory != null &&
                                                                    f.FamilyCategory.CategoryType == CategoryType.Annotation &&
                                                                    f.Name == stoc_Designation)
                                                        .FirstOrDefault();
                            // Делаем его общим
                            if (annotationFamily != null)
                            {
                                Parameter parameter = annotationFamily.get_Parameter(BuiltInParameter.FAMILY_SHARED);
                                parameter.Set(1);
                            }
                            Document doc_Annotation = doc_Family.EditFamily(annotationFamily);

                            string nestedFamilyPath = Path.Combine(Path.GetTempPath(), annotationFamily.Name + ".rfa");
                            if (!File.Exists(nestedFamilyPath))
                            {
                                using (Transaction trans = new Transaction(document_Target, "Сохранение семейства"))
                                {
                                    trans.Start();
                                    SaveAsOptions options = new SaveAsOptions();
                                    options.OverwriteExistingFile = true;
                                    doc_Annotation.SaveAs(nestedFamilyPath, options);
                                    doc_Annotation.Close(false);
                                    trans.Commit();
                                }
                            }
                            doc_Family.Close(false);
                            int a = 0;

                            using (Transaction trans = new Transaction(document_Target, "Загрузка семейства"))
                            {
                                trans.Start();
                                doc.LoadFamily(nestedFamilyPath, out loadedFamily);
                                trans.Commit();
                            }


                            //using (Transaction t = new Transaction(doc_Annotation, "Сделать семейство общим"))
                            //{
                            //    t.Start();

                            //    // Устанавливаем флаг "Общее семейство"
                            //    Document familyDoc = uiApp.Application.OpenDocumentFile(nestedFamilyPath);

                            //    if (familyDoc == null || !familyDoc.IsFamilyDocument)
                            //    {
                            //        TaskDialog.Show("Revit", "Ошибка: Невозможно открыть файл семейства.");
                            //        return;
                            //    }

                                

                            //        // Получаем текущее семейство
                            //        Family familyas = familyDoc.OwnerFamily;

                            //        // Делаем его общим
                            //        if (familyas != null)
                            //        {
                            //            Parameter parameter = familyas.get_Parameter(BuiltInParameter.FAMILY_SHARED);
                            //            parameter.Set(1); 
                            //        }


                            //    // Сохраняем семейство с заменой файла
                            //    SaveAsOptions options = new SaveAsOptions();
                            //    options.OverwriteExistingFile = true;
                            //    familyDoc.SaveAs(nestedFamilyPath, options);

                            //    // Закрываем семейство
                            //    familyDoc.Close(false);

                            //    t.Commit();
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                        S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                        s_Mistake_String.ShowDialog();
                    }
                    if (parameter_Position.element_Type_On_Ex == "Тип")
                    {
                        using (Transaction trans = new Transaction(document_Target, "Обновление параметров"))
                        {
                            trans.Start();

                            if (param != null && !param.IsReadOnly)
                            {
                                if (param.StorageType == StorageType.Integer)
                                    param.Set(parameter_Position.bool_Value == 1 ? 1 : 0);
                                else if (param.StorageType == StorageType.Double)
                                    param.Set(parameter_Position.double_Value);
                                else if (param.StorageType == StorageType.ElementId && param.Definition.GetDataType().TypeId == "autodesk.spec.aec:material-1.0.0")
                                {
                                    Material material = new FilteredElementCollector(document_Target).OfClass(typeof(Material)).Cast<Material>().FirstOrDefault(m => m.Name == parameter_Position.material_Value);
                                    param.Set(material.Id);
                                }
                                //else if (param.StorageType == StorageType.ElementId && param.Definition.GetDataType().TypeId == "autodesk.revit.category.family:genericAnnotation-1.0.0" && element.Id == parameter_Position.parameter.Id)
                                //{
                                //    string name = param.Definition.Name;
                                //    ElementId elemId = document_Donor.GetElement(document_Donor.GetElement(Data_Class_Property_Copy.element_Donor.GetTypeId()).Id).LookupParameter(param.Definition.Name).AsElementId();
                                //    param.Set(elemId);
                                //}
                                else
                                {
                                    try
                                    {
                                        Family annotationFamily = new FilteredElementCollector(document_Target)
                                                        .OfClass(typeof(Family))
                                                        .Cast<Family>()
                                                        .Where(f => f.FamilyCategory != null &&
                                                                    f.FamilyCategory.CategoryType == CategoryType.Annotation &&
                                                                    f.Name == stoc_Designation)
                                                        .FirstOrDefault();
                                        FamilySymbol familySymbol = annotationFamily.GetFamilySymbolIds()
                                        .Select(id => document_Target.GetElement(id) as FamilySymbol)
                                        .FirstOrDefault(fs => fs != null && fs.Name == stoc_Type_Name);
                                        string a = familySymbol.Name;
                                        string nameParam = param.Definition.Name;
                                        //param.Set(familySymbol.Id);
                                        //                                   FamilyManager familyManager = document_Target.FamilyManager;
                                        //                                   FamilyParameter familyTypeParam = familyManager.Parameters
                                        //.Cast<FamilyParameter>()
                                        //.FirstOrDefault(p => p.ParameterType == ParameterType.FamilyType && p.Definition.Name == paramName);
                                        //FamilyParameter familyTypeParam = familyManager.Parameters
                                        //.Cast<FamilyParameter>()
                                        //.FirstOrDefault(p => p.Definition.ParameterType == ParameterType.FamilyType && p.Definition.Name == paramName);

                                        param.Set(new ElementId(27796493));
                                    }

                                    catch (Exception ex)
                                    {
                                        S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                                        s_Mistake_String.ShowDialog();
                                    }
                                }
                            }
                            document_Target.Regenerate();
                            //document.RefreshActiveView();
                            trans.Commit();
                        }
                    }
                    else
                    {
                        Parameter param_ex = element.LookupParameter(parameter_Position.parameter.Definition.Name);
                        if (param_ex != null && !param_ex.IsReadOnly)
                        {

                            if (param_ex.StorageType == StorageType.Integer)
                                param_ex.Set(parameter_Position.bool_Value == 1 ? 1 : 0);
                            else if (param_ex.StorageType == StorageType.Double)
                                param_ex.Set(parameter_Position.double_Value);
                            else if (param_ex.StorageType == StorageType.ElementId)
                            {
                                Material material_ex = new FilteredElementCollector(document_Target).OfClass(typeof(Material)).Cast<Material>().FirstOrDefault(m => m.Name == parameter_Position.material_Value);
                                param_ex.Set(material_ex.Id);
                            }
                        }
                    }
                }
            }

               
        }

        public string GetName() => "PropertyCopyHandler";

        public void SetData(List<Element> elements, List<Parameter_Identification> parameters,Document doc_Donor , Document doc_Target)
        {
            this.elements = elements;
            this.parameters = parameters;
            this.document_Donor = doc_Donor;
            this.document_Target = doc_Target;
        }
    }
    public class FamilyTypeSetter
    {
        public static void SetNestedFamilyType(Document doc, FamilyInstance instance, string parameterName, string targetTypeName)
        {
            // Получаем параметр "Типоразмеры в проекте"
            Parameter param = instance.LookupParameter(parameterName);
            if (param == null)
            {
                //TaskDialog.Show("Ошибка", $"Параметр '{parameterName}' не найден.");
                //return;
            }

            // Проверяем, является ли параметр типа FamilyType
            

            // Получаем FamilySymbol родительского семейства
            FamilySymbol hostSymbol = instance.Symbol;
            if (hostSymbol == null)
            {
                //TaskDialog.Show("Ошибка", "Не удалось получить FamilySymbol хост-семейства.");
                //return;
            }

            Family parentFamily = hostSymbol.Family;
            if (parentFamily == null)
            {
                //TaskDialog.Show("Ошибка", "Не удалось определить родительское семейство.");
                //return;
            }

            // Получаем все доступные типы вложенного семейства в рамках родительского
            List<FamilySymbol> availableTypes = new List<FamilySymbol>();
            foreach (ElementId typeId in parentFamily.GetFamilySymbolIds())
            {
                FamilySymbol symbol = doc.GetElement(typeId) as FamilySymbol;
                if (symbol != null)
                {
                    availableTypes.Add(symbol);
                }
            }

            // Ищем нужный тип
            FamilySymbol targetType = availableTypes.FirstOrDefault(f => f.Name == targetTypeName);
            if (targetType == null)
            {
                TaskDialog.Show("Ошибка", $"Тип '{targetTypeName}' не найден в семействе '{parentFamily.Name}'.");
                return;
            }

            // Записываем в параметр
            using (Transaction trans = new Transaction(doc, "Изменение типа вложенного семейства"))
            {
                trans.Start();
                param.Set(targetType.Id);
                trans.Commit();
            }

            TaskDialog.Show("Успех", $"Тип '{targetTypeName}' успешно установлен в параметр '{parameterName}'.");
        }
    }
}
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
using System.Windows;
using Application = Autodesk.Revit.ApplicationServices.Application;
using FerrumAddin;


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
            SSDK_Data.licenses_Name = Environment.UserName;
            Data_Class_Property_Copy.element_Donor = null;
            Data_Class_Property_Copy.elements_Target.Items.Clear();
            Data_Class_Property_Copy.elements_Target_Elements.Clear();
            if (SSDK_Data.licenses_Connection)
            {
                UIApplication uiApp = commandData.Application;
                Application application = uiApp.Application;
                UIDocument uidoc = uiApp.ActiveUIDocument;
                Document_Property_Copy_Donor.Document = uidoc.Document;
                Document_Property_Copy_Target.Document = uidoc.Document;
           
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
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. Ваша лицензия недоступна. Выполните переподключение");
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
            //Формируем списки заготовки
            List<Parameter_Identification> parameters_Ghost = new List<Parameter_Identification>(); /*Группа параметров по гост*/
            List<Parameter_Identification> parameters_Materials_Type = new List<Parameter_Identification>(); /*Группа параметров по материалам*/
            List<Parameter_Identification> parameters_Double_Type = new List<Parameter_Identification>(); /*Группа параметров по длине*/
            List<Parameter_Identification> parameters_Bool_Type = new List<Parameter_Identification>(); /*Группа параметров по Да/Нет*/
            List<Parameter_Identification> parameters_Materials_Ex = new List<Parameter_Identification>(); /*Группа параметров по материалам*/
            List<Parameter_Identification> parameters_Double_Ex = new List<Parameter_Identification>(); /*Группа параметров по длине*/
            List<Parameter_Identification> parameters_Bool_Ex = new List<Parameter_Identification>(); /*Группа параметров по Да/Нет*/
            List<Element> element_Type_List = new List<Element>(); /*Группа элементов по типу*/
            //Распределяем параметры по группам
            foreach (Parameter_Identification parameter_Identification in parameters) /*Цикл по параметрам после анализа*/
            {
                if (parameter_Identification.type_Parameter == "ghost") { parameters_Ghost.Add(parameter_Identification); } /*Наполнение списка параметров по гост*/
                else if (parameter_Identification.type_Parameter == "material" && parameter_Identification.element_Type_On_Ex == "Тип") { parameters_Materials_Type.Add(parameter_Identification); } /*Наполнение списка параметров по материалу в типе*/
                else if (parameter_Identification.type_Parameter == "size" && parameter_Identification.element_Type_On_Ex == "Тип") { parameters_Double_Type.Add(parameter_Identification); } /*Наполнение списка параметров по длине в типе*/
                else if (parameter_Identification.type_Parameter == "bool" && parameter_Identification.element_Type_On_Ex == "Тип") { parameters_Bool_Type.Add(parameter_Identification); } /*Наполнение списка параметров по Да/Нет в типе*/
                else if (parameter_Identification.type_Parameter == "material" && parameter_Identification.element_Type_On_Ex == "Экземпляр") { parameters_Materials_Ex.Add(parameter_Identification); } /*Наполнение списка параметров по материалу в экземпляре*/
                else if (parameter_Identification.type_Parameter == "size" && parameter_Identification.element_Type_On_Ex == "Экземпляр") { parameters_Double_Ex.Add(parameter_Identification); } /*Наполнение списка параметров по длине в экземпляре*/
                else if (parameter_Identification.type_Parameter == "bool" && parameter_Identification.element_Type_On_Ex == "Экземпляр") { parameters_Bool_Ex.Add(parameter_Identification); } /*Наполнение списка параметров по Да/Нет в экземпляре*/
            }
            try
            {
                using (TransactionGroup transactionGroup = new TransactionGroup(document_Target, "Копирование свойств"))
                {
                    transactionGroup.Start();
                    Material_Download_Collection(parameters_Materials_Type, parameters_Materials_Ex, document_Donor, document_Target);
                    element_Type_List = elements.GroupBy(p => p.GetTypeId()).Select(g => g.First()).ToList(); /*Наполнение уникальных типов по выбранным элементам*/
                    if (element_Type_List.Count > 0 && parameters_Materials_Type.Count > 0 || parameters_Double_Type.Count > 0 || parameters_Bool_Type.Count > 0 || parameters_Ghost.Count > 0)
                    {
                        if (parameters_Ghost.Count > 0) /*Проверка списка типов и параметров гост элемента донора выбранных элементов на количество*/
                        {
                            List<(string, string)> parameter_Name_And_Value = new List<(string, string)>();
                            string family_Name = "";
                            string nestedFamilyPath = "";
                            foreach (Parameter_Identification parameter_Identification in parameters_Ghost)
                            {
                                string param_Value = parameter_Identification.parameter.AsValueString();
                                string[] param_Array = param_Value.Split(new[] { " : " }, StringSplitOptions.None);
                                family_Name = param_Array[0];
                                string param_Value_Name = param_Array[param_Array.Count() - 1];
                                parameter_Name_And_Value.Add((parameter_Identification.parameter.Definition.Name, param_Value_Name));
                            }
                            foreach (Element element_Type_Position in element_Type_List)
                            {
                                FamilySymbol familySymbol = document_Target.GetElement(element_Type_Position.GetTypeId()) as FamilySymbol;
                                Family family = familySymbol.Family;
                                Document doc_Family = document_Target.EditFamily(family);
                                Family loadedFamily;
                                Family annotationFamily = new FilteredElementCollector(doc_Family).OfClass(typeof(Family)).Cast<Family>().Where(f =>
                                f.FamilyCategory != null && f.FamilyCategory.CategoryType == CategoryType.Annotation && f.Name == family_Name).FirstOrDefault();
                                nestedFamilyPath = Path.Combine(Path.GetTempPath(), annotationFamily.Name + ".rfa");
                                Document doc_Annotation = doc_Family.EditFamily(annotationFamily);
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
                                using (Transaction trans = new Transaction(document_Target, "Загрузка семейства"))
                                {
                                    trans.Start();
                                    document_Target.LoadFamily(nestedFamilyPath, out loadedFamily);
                                    trans.Commit();
                                }
                                Family annotationDownloadOnProgect = new FilteredElementCollector(document_Target).OfClass(typeof(Family)).Cast<Family>()
                                .Where(f => f.FamilyCategory != null && f.FamilyCategory.CategoryType == CategoryType.Annotation && f.Name == family_Name).FirstOrDefault();
                                using (Transaction trans_Shared = new Transaction(document_Target, "Делаем семейство общим"))
                                {
                                    trans_Shared.Start();
                                    if (annotationDownloadOnProgect != null)
                                    {
                                        Parameter parameter = annotationDownloadOnProgect.get_Parameter(BuiltInParameter.FAMILY_SHARED);
                                        parameter.Set(1);
                                    }
                                    document_Target.Regenerate();
                                    trans_Shared.Commit();
                                }
                                List<FamilySymbol> familySymbols = annotationDownloadOnProgect.GetFamilySymbolIds()
                                .Select(id => document_Target.GetElement(id) as FamilySymbol).Where(symbol => symbol != null).ToList();
                                using (Transaction trans_Set = new Transaction(document_Target, "Записываем значение в параметр"))
                                {
                                    trans_Set.Start();
                                    ElementId elem = new ElementId(391308);
                                    foreach ((string parametername, string value) in parameter_Name_And_Value)
                                    {
                                        foreach (FamilySymbol familyTypePosition in familySymbols)
                                        {
                                            if (value == familyTypePosition.Name)
                                            {
                                                familySymbol.LookupParameter(parametername).Set(familyTypePosition.Id);
                                            }
                                        }
                                        
                                    }
                                    trans_Set.Commit();
                                }
                                using (Transaction trans_Delete = new Transaction(document_Target, "Семейство из проекта"))
                                {
                                    trans_Delete.Start();
                                    document_Target.Delete(annotationDownloadOnProgect.Id);
                                    trans_Delete.Commit();
                                }
                                if (File.Exists(nestedFamilyPath)) { File.Delete(nestedFamilyPath); }
                            }
                        }
                        if (parameters_Materials_Type.Count > 0 || parameters_Double_Type.Count > 0 || parameters_Bool_Type.Count > 0)
                        {
                            using (Transaction trans_Set_Parameter_Type = new Transaction(document_Target, "Записываем значение в параметр"))
                            {
                                trans_Set_Parameter_Type.Start();
                                foreach (Element element in element_Type_List)
                                {
                                    Element element_Type = document_Target.GetElement(element.GetTypeId());
                                    foreach (Parameter_Identification parameter_Identification_Material in parameters_Materials_Type)
                                    {
                                        if (element_Type.LookupParameter(parameter_Identification_Material.parameter.Definition.Name) != null)
                                        {
                                            Material material = new FilteredElementCollector(document_Target).OfClass(typeof(Material)).Cast<Material>().FirstOrDefault(m => m.Name == parameter_Identification_Material.material_Value);
                                            element_Type.LookupParameter(parameter_Identification_Material.parameter.Definition.Name).Set(material.Id);
                                        }
                                    }
                                    foreach (Parameter_Identification parameter_Identification_Double in parameters_Double_Type)
                                    {
                                        if (element_Type.LookupParameter(parameter_Identification_Double.parameter.Definition.Name) != null)
                                        {
                                            element_Type.LookupParameter(parameter_Identification_Double.parameter.Definition.Name).Set(parameter_Identification_Double.double_Value);
                                        }
                                    }
                                    foreach (Parameter_Identification parameter_Identification_Bool in parameters_Bool_Type)
                                    {
                                        if (element_Type.LookupParameter(parameter_Identification_Bool.parameter.Definition.Name) != null)
                                        {
                                            element_Type.LookupParameter(parameter_Identification_Bool.parameter.Definition.Name).Set(parameter_Identification_Bool.bool_Value == 1 ? 1 : 0);
                                        }
                                    }
                                }
                                trans_Set_Parameter_Type.Commit();
                            }
                        }
                    }
                    if (parameters_Materials_Ex.Count > 0 || parameters_Double_Ex.Count > 0 || parameters_Bool_Ex.Count > 0)
                    {
                        using (Transaction trans_Set_Parameter_Ex = new Transaction(document_Target, "Записываем значение в параметр"))
                        {
                            trans_Set_Parameter_Ex.Start();
                            foreach (Element element in elements)
                            {
                                foreach (Parameter_Identification parameter_Identification_Material in parameters_Materials_Ex)
                                {
                                    if (element.LookupParameter(parameter_Identification_Material.parameter.Definition.Name) != null)
                                    {
                                        Material material = new FilteredElementCollector(document_Target).OfClass(typeof(Material)).Cast<Material>().FirstOrDefault(m => m.Name == parameter_Identification_Material.material_Value);
                                        element.LookupParameter(parameter_Identification_Material.parameter.Definition.Name).Set(material.Id);
                                    }
                                }
                                foreach (Parameter_Identification parameter_Identification_Double in parameters_Double_Ex)
                                {
                                    if (element.LookupParameter(parameter_Identification_Double.parameter.Definition.Name) != null)
                                    {
                                        element.LookupParameter(parameter_Identification_Double.parameter.Definition.Name).Set(parameter_Identification_Double.double_Value);
                                    }
                                }
                                foreach (Parameter_Identification parameter_Identification_Bool in parameters_Bool_Ex)
                                {
                                    if (element.LookupParameter(parameter_Identification_Bool.parameter.Definition.Name) != null)
                                    {
                                        element.LookupParameter(parameter_Identification_Bool.parameter.Definition.Name).Set(parameter_Identification_Bool.bool_Value == 1 ? 1 : 0);
                                    }
                                }
                            }
                            trans_Set_Parameter_Ex.Commit();
                        }
                    }
                    transactionGroup.Assimilate();
                }
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Запись завершена. Успешно обработаных элементов: " + Data_Class_Property_Copy.elements_Target_Elements.Count.ToString());
                s_Mistake_String.ShowDialog();
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
        }
        public string GetName() => "PropertyCopyHandler";
        public void SetData(List<Element> elements, List<Parameter_Identification> parameters, Document doc_Donor, Document doc_Target)
        {
            this.elements = elements;
            this.parameters = parameters;
            this.document_Donor = doc_Donor;
            this.document_Target = doc_Target;
        }
        public void Material_Download_Collection(List<Parameter_Identification> parameters_Materials_Type , List<Parameter_Identification> parameters_Materials_Ex, Document document_Donor ,Document document_Target) {
            if (parameters_Materials_Type.Count > 0 || parameters_Materials_Ex.Count > 0) {
                List<ElementId> elementIds = new List<ElementId>();
                foreach (Parameter_Identification parameter_Identification in parameters_Materials_Type) {
                    elementIds = new List<ElementId>();
                    if (!new FilteredElementCollector(document_Target).OfClass(typeof(Material)).Cast<Material>().Any(m => m.Name.Equals(parameter_Identification.parameter.AsValueString()))){
                        elementIds.Add(parameter_Identification.parameter.AsElementId());
                        Material_Download_Transaction(elementIds , document_Donor, document_Target);
                    }
                }
                foreach (Parameter_Identification parameter_Identification in parameters_Materials_Ex) {
                    elementIds = new List<ElementId>();
                    if (!new FilteredElementCollector(document_Target).OfClass(typeof(Material)).Cast<Material>().Any(m => m.Name.Equals(parameter_Identification.parameter.AsValueString()))){
                        elementIds.Add(parameter_Identification.parameter.AsElementId());
                        Material_Download_Transaction(elementIds, document_Donor, document_Target);
                    }
                }
            }
        }
        public void Material_Download_Transaction(List<ElementId> elementIds, Document document_Donor, Document document_Target) {
            using (Transaction trans_Material = new Transaction(document_Target, "Загрузка материалов")) {
                trans_Material.Start();
                CopyPasteOptions copyPasteOptions = new CopyPasteOptions();
                ElementTransformUtils.CopyElements(document_Donor, elementIds, document_Target, Transform.Identity, copyPasteOptions);
                trans_Material.Commit();
            }
        }
    }
}
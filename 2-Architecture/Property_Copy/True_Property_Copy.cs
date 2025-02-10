using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPFApplication.Property_Copy;

namespace WPFApplication.Property_Copy
{
    public class LoadElementParameters
    {
        public LoadElementParameters(Element element)
        {
            Data_Class_Property_Copy.ParameterCategories.Clear();

            var typeCategory = new ParameterCategory("Тип");
            var instanceCategory = new ParameterCategory("Экземпляр");

            var size_Group = new ParameterGroup("Размеры");
            var material_Group = new ParameterGroup("Материалы");
            var material_Visibilite = new ParameterGroup("Видимость");

            // Загружаем параметры типа
            ElementId typeId = element.GetTypeId();
            if (typeId != ElementId.InvalidElementId)
            {
                Element typeElement = Document_Property_Copy.Document.GetElement(typeId);
                AddParametersToCategory(typeElement, size_Group, typeCategory);
                AddParametersToCategory(typeElement, material_Group, typeCategory);
                AddParametersToCategory(typeElement, material_Visibilite, typeCategory);
            }

            // Загружаем параметры экземпляра
            AddParametersToCategory(element, size_Group, instanceCategory);
            AddParametersToCategory(element, material_Group, instanceCategory);
            AddParametersToCategory(element, material_Visibilite, instanceCategory);

            Data_Class_Property_Copy.ParameterCategories.Add(typeCategory);
            Data_Class_Property_Copy.ParameterCategories.Add(instanceCategory);


        }
        private void AddParametersToCategory(Element element, ParameterGroup parameterGroup, ParameterCategory category)
        {
            ParameterGroup parameterGroupEx = new ParameterGroup(parameterGroup.Name);
            foreach (Parameter param in element.Parameters.Cast<Parameter>())
            {
                string paramName = param.Definition.Name;
                string paramValue = param.AsValueString() ?? param.AsString() ?? "—";
                if (parameterGroup.Name == "Размеры" && param.Definition.ParameterGroup == BuiltInParameterGroup.PG_GEOMETRY && param.Definition.GetDataType().TypeId == "autodesk.spec.aec:length-2.0.0")
                {
                    parameterGroupEx.Parameters.Add(new ParameterItem($"{paramName}: {paramValue}", paramName));
                }
                if (parameterGroup.Name == "Материалы" && param.Definition.ParameterGroup == BuiltInParameterGroup.PG_MATERIALS && param.Definition.GetDataType().TypeId == "autodesk.spec.aec:material-1.0.0")
                {
                    parameterGroupEx.Parameters.Add(new ParameterItem($"{paramName}: {paramValue}", paramName));
                }
                if (parameterGroup.Name == "Видимость" && param.Definition.ParameterGroup == BuiltInParameterGroup.PG_VISIBILITY && param.Definition.GetDataType().TypeId == "autodesk.spec:spec.bool-1.0.0")
                {
                    parameterGroupEx.Parameters.Add(new ParameterItem($"{paramName}: {paramValue}", paramName));
                }

            }
            category.ParametersGroup.Add(parameterGroupEx);
        }
    }
    public class LoadParameters
    {
        public Element element_Donor { get; set; } = null;
        public IList<Parameter_Identification> LoadParameters_Position(Element element, TreeView treeViewItem)
        {
            element_Donor = element;
            IList < Parameter_Identification > parameters = new List<Parameter_Identification>();   
            foreach (ParameterCategory items_Category in treeViewItem.Items)
            {
                if (items_Category.Name == "Тип")
                {
                    foreach (ParameterGroup items_Group in items_Category.ParametersGroup)
                    {
                        ProccessTreeViewItemType(items_Group, element_Donor,  parameters , "Тип");
                    }
                }
            }
            return parameters;
            
        }
        public void ProccessTreeViewItemType(ParameterGroup treeViewItem, Element element_Donor, IList<Parameter_Identification> parameter_Identifications, string element_Type_On_Ex)
        {
            foreach(ParameterItem items in treeViewItem.Parameters)
            {
                if (items.IsChecked == true)
                {
                    Element elementType = Document_Property_Copy.Document.GetElement(element_Donor.GetTypeId());
                    foreach (Parameter parameter in elementType.Parameters)
                    {
                        if (parameter.Definition.Name == items.Name)
                        {
                            if (parameter.Definition.GetDataType().TypeId == "autodesk.spec:spec.bool-1.0.0")
                            {
                                int ischecked = parameter.AsInteger();
                                Parameter_Identification parameter_Identification = new Parameter_Identification("bool", parameter, ischecked, element_Type_On_Ex);
                                parameter_Identifications.Add(parameter_Identification);
                            }
                            if (parameter.Definition.GetDataType().TypeId == "autodesk.spec.aec:material-1.0.0")
                            {
                                string ischecked = parameter.AsValueString();
                                Parameter_Identification parameter_Identification = new Parameter_Identification("material", parameter, ischecked, element_Type_On_Ex);
                                parameter_Identifications.Add(parameter_Identification);
                            }
                            if (parameter.Definition.GetDataType().TypeId == "autodesk.spec.aec:length-2.0.0")
                            {
                                double ischecked = parameter.AsDouble();
                                Parameter_Identification parameter_Identification = new Parameter_Identification("size", parameter, ischecked, element_Type_On_Ex);
                                parameter_Identifications.Add(parameter_Identification);
                            }
                        }
                    }
                }
            }
        }
       
//         if (parameter.Definition.GetDataType().TypeId == "autodesk.spec:spec.bool-1.0.0")
//                            {
//                                int ischecked = parameter.AsInteger();
//        Parameter_Identification parameter_Identification = new Parameter_Identification("bool", parameter, ischecked, element_Type_On_Ex);
//        parameter_Identifications.Add(parameter_Identification);
//                            }
//                            if (parameter.Definition.GetDataType().TypeId == "autodesk.spec.aec:material-1.0.0")
//                            {
//                                string ischecked = parameter.AsValueString();
//    Parameter_Identification parameter_Identification = new Parameter_Identification("bool", parameter, ischecked, element_Type_On_Ex);
//    parameter_Identifications.Add(parameter_Identification);
//                            }
//if (parameter.Definition.GetDataType().TypeId == "autodesk.spec.aec:length-2.0.0")
//{
//    double ischecked = parameter.AsDouble();
//    Parameter_Identification parameter_Identification = new Parameter_Identification("bool", parameter, ischecked, element_Type_On_Ex);
//    parameter_Identifications.Add(parameter_Identification);
//}
        // Метод для получения всех элементов с состоянием чекбокса
        //public List<(string Name, bool IsChecked)> GetCheckedItems()
        //{
        //    List<(string Name, bool IsChecked)> result = new List<(string Name, bool IsChecked)> ();

        //    foreach (var category in Categories)
        //    {
        //        foreach (var group in category.ParametersGroup)
        //        {
        //            foreach (var param in group.Parameters)
        //            {
        //                result.Add((param.DisplayText, param.IsChecked));
        //            }
        //        }
        //    }

        //    return result;
        //}
    }
    public class Ser_Parameter_On_Target
    {
         public void Ser_Parameter_Target(IList<Parameter_Identification> parameter_Identifications, List<Element> elements)
        {
            foreach (Element element in elements)
            {
                foreach (Parameter_Identification parameter_Position in parameter_Identifications)
                {
                    //if (parameter_Position.element_Type_On_Ex == "Тип" && Document_Property_Copy.Document.GetElement(element.GetTypeId()) != null)
                    //{
                        Element element_type = Document_Property_Copy.Document.GetElement(element.GetTypeId());
                        if (parameter_Position.type_Parameter == "bool")
                        {
                            element_type.LookupParameter(parameter_Position.parameter.Definition.Name).Set(parameter_Position.bool_Value);
                        }
                        if (parameter_Position.type_Parameter == "material")
                        {
                            //element_type.LookupParameter(parameter_Position.parameter.Definition.Name).Set(parameter_Position.bool_Value);
                        }
                        if (parameter_Position.type_Parameter == "size")
                        {
                            element_type.LookupParameter(parameter_Position.parameter.Definition.Name).Set(parameter_Position.double_Value);
                        }
                    //}
                }
            }
        }
    }
}

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
        public LoadParameters(Element element, TreeViewItem treeViewItem, IList<Parameter_Identification> parameter_Identifications)
        {
            element_Donor = element;    
            foreach (ParameterCategory items_Category in treeViewItem.Items)
            {
                if (items_Category.Name == "Тип")
                {
                    foreach (ParameterGroup items_Group in items_Category.ParametersGroup)
                    {
                        ProccessTreeViewItemType(items_Group,  parameter_Identifications);
                    }
                }
            }
            
        }
        public void ProccessTreeViewItemType(ParameterGroup treeViewItem, IList<Parameter_Identification> parameter_Identifications)
        {
            foreach(ParameterItem items in treeViewItem.Parameters)
            {
                if (items.IsChecked == true)
                {
                    foreach (Parameter parameter in element_Donor.Parameters)
                    {
                        if (parameter.Definition.Name == items.Name)
                        {
                            if (parameter.Definition.GetDataType().TypeId == "autodesk.spec:spec.bool-1.0.0")
                            {
                                int ischecked = parameter.AsInteger();
                                Parameter_Identification parameter_Identification = new Parameter_Identification("bool", parameter, ischecked);
                                parameter_Identifications.Add(parameter_Identification);
                            }
                            if (parameter.Definition.GetDataType().TypeId == "autodesk.spec.aec:material-1.0.0")
                            {
                                string ischecked = parameter.AsValueString();
                                Parameter_Identification parameter_Identification = new Parameter_Identification("bool", parameter, ischecked);
                                parameter_Identifications.Add(parameter_Identification);
                            }
                            if (parameter.Definition.GetDataType().TypeId == "autodesk.spec.aec:length-2.0.0")
                            {
                                double ischecked = parameter.AsDouble();
                                Parameter_Identification parameter_Identification = new Parameter_Identification("bool", parameter, ischecked);
                                parameter_Identifications.Add(parameter_Identification);
                            }
                        }
                    }
                }
            }
        }
    }
}

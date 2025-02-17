using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPFApplication.Property_Copy
{
    public class Data_Class_Property_Copy
    {
        public static Guid zh_Cod { get; set; } = new Guid("631cd69e-065f-4ec2-8894-4359325312c3");
        public static FamilyInstance element_Donor { get; set; } = null;
        public static ListView elements_Target { get; set; } = new ListView();
        public static List<Element> elements_Target_Elements { get; set; } = new List<Element>();
        public static ObservableCollection<ParameterCategory> ParameterCategories { get; set; }

        public static IList<Parameter_Identification> parameters = new List<Parameter_Identification>();

        public static IList<Parameter_Identification> parameters_Ex = new List<Parameter_Identification>();

        public static IList<Document> all_Document = new List<Document>();
    }
    //public static class Document_Property_Copy
    //{
    //    public static IList<Document> all_Document = new List<Document>();
    //    public static UIApplication UIApplication { get; set; }
    //    public static UIDocument UIDobument { get => UIApplication.ActiveUIDocument; }
    //    public static Document Document { get => UIDobument.Document; }
    //    public static ExternalCommandData Data { get; set; }
    //    public static void Initialize(ExternalCommandData commandData)
    //    {
    //        Data = commandData;
    //        UIApplication = commandData.Application;
    //    }
    //}
    public static class Document_Property_Copy_Donor
    {
        
        public static Document Document { get; set; } 
        public static void Initialize(Document doc)
        {
            Document = doc;
        }
    }
    public static class Document_Property_Copy_Target
    {

        public static Document Document { get; set; }
        public static void Initialize(Document doc)
        {
            Document = doc;
        }
    }
    public class ParameterCategory
    {
        public string Name { get; set; }
        public ObservableCollection<ParameterGroup> ParametersGroup { get; set; }

        public ParameterCategory(string name)
        {
            Name = name;
            ParametersGroup = new ObservableCollection<ParameterGroup>();
        }
    }
    public class ParameterGroup : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public ObservableCollection<ParameterItem> Parameters { get; set; }

        private bool _isCheckedGroup;
        public bool IsCheckedGroup
        {
            get => _isCheckedGroup;
            set
            {
                if (_isCheckedGroup != value)
                {
                    _isCheckedGroup = value;
                    OnPropertyChanged(nameof(IsCheckedGroup));

                    // Устанавливаем галочку у всех параметров внутри группы
                    foreach (var param in Parameters)
                    {
                        param.IsChecked = value;
                    }
                }
            }
        }

        public ParameterGroup(string name)
        {
            Name = name;
            Parameters = new ObservableCollection<ParameterItem>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public class ParameterItem : INotifyPropertyChanged
    {
        public string DisplayText { get; set; }
        public string Name { get; set; }

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }

        public ParameterItem(string text, string name) 
        {
            DisplayText = text;
            IsChecked = false;
            Name = name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public class Parameter_Identification
    {
        public string element_Type_On_Ex { get; set; } = "";
        public  string type_Parameter = null;
        public  Parameter parameter { get; set; } = null;
        public  double double_Value = 0;
        public  string material_Value = null;
        public string ghost_Value = null;
        public  int bool_Value = 0;
        public Parameter_Identification(string type_Value,Parameter parameter_Value, double value_Value, string element_Type_On_Ex_Value)
        {
            type_Parameter = type_Value;
            parameter = parameter_Value;
            double_Value = value_Value;
            element_Type_On_Ex = element_Type_On_Ex_Value;
        }
        public Parameter_Identification(string type_Value, Parameter parameter_Value, string value_Value, string element_Type_On_Ex_Value)
        {
            if (parameter_Value.Definition.GetDataType().TypeId == "autodesk.spec.aec:material-1.0.0")
            {
                type_Parameter = type_Value;
                parameter = parameter_Value;
                material_Value = value_Value;
                element_Type_On_Ex = element_Type_On_Ex_Value;
            }
            if (parameter_Value.Definition.GetDataType().TypeId == "autodesk.revit.category.family:genericAnnotation-1.0.0")
            {
                type_Parameter = type_Value;
                parameter = parameter_Value;
                string[] value_Reduction = value_Value.Split(new[] { ":" }, StringSplitOptions.None);
                ghost_Value = value_Reduction[value_Reduction.Count() - 1];
                element_Type_On_Ex = element_Type_On_Ex_Value;
            }
        }
        public Parameter_Identification(string type_Value, Parameter parameter_Value, int value_Value, string element_Type_On_Ex_Value)
        {
            type_Parameter = type_Value;
            parameter = parameter_Value;
            bool_Value = value_Value;
            element_Type_On_Ex = element_Type_On_Ex_Value;
        }
    }
}

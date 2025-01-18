using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPFApplication.Parameter_Door
{
    public class Data_Parameter_Door
    {
        public static List<Element> filtered_Group { get; set; } = new List<Element>();
        public static Guid guid_ADSK_Desination = new Guid("9c98831b-9450-412d-b072-7d69b39f4029");
        public static Guid guid_COD = new Guid("631cd69e-065f-4ec2-8894-4359325312c3");
        public static Guid guid_ADSK_NAME = new Guid("e6e0f5cd-3e26-485b-9342-23882b20eb43");
        public static Guid guid_ADSK_Mark = new Guid("2204049c-d557-4dfc-8d70-13f19715e46d");
        public static List<Glass_Window> list_Group { get; set; } = new List<Glass_Window>();
        public static int number_Elements { get; set; } = 0;
        public static bool iteration_Recaive_Value_In_Parameter = false;
        public static bool error_Suppressio = false;
        public static string iteration_Recaive_Value_In_Parameter_Watringn = "";
    }
    public static class Revit_Document_Parameter_Window
    {
        public static UIApplication UIApplication { get; set; }
        public static UIDocument UIDobument { get => UIApplication.ActiveUIDocument; }
        public static Document Document { get => UIDobument.Document; }
        public static ExternalCommandData Data { get; set; }
        public static void Initialize(ExternalCommandData commandData)
        {
            Data = commandData;
            UIApplication = commandData.Application;
        }
    }
    public class Glass_Window
    {
        public string description_Value { get; set; } = "";
        public string model_Value { get; set; } = "";
        public string model_Designation { get; set; } = "";
        public string height_Value { get; set; } = "";
        public string wight_Value { get; set; } = "";
        public Element element { get; set; } = null;
        public string full_Name { get; set; } = "";
        
        public Glass_Window(string description_Value, string model_Value, string model_Designation, string height_Value, string wight_Value, Element element,string full_Name)
        {
            this.description_Value = description_Value;
            this.model_Value = model_Value;
            this.model_Designation = model_Designation;
            this.height_Value = height_Value;
            this.wight_Value = wight_Value;
            this.element = element;
            this.full_Name = full_Name;
        }
    }
}

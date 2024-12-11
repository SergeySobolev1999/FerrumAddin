using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPFApplication.SharedParameterManager;
using System.Windows.Markup;

namespace RevitAPI_Basic_Course.Creating_Specifications
{
    public class Data_Creating_Specifications
    {
        public static string Folder_Base_Way {  get; set; }
        public static string Folder_Presets { get; set; }
        public static ICollection<Element> sheets_Filter { get; set; } = new List<Element>();
        public static ListView list_Filter1 { get; set; } = new ListView();
        public static ListView list_Filter2 { get; set; } = new ListView();
        public static string filter_Text_Sheets { get; set; } = "";
        public static List<string> list_Filter_Model { get; set; } = new List<string>();

        public static Scrol_Viewer1_Creating_Specifications scrol_Viewer1_Creating_Specifications = new Scrol_Viewer1_Creating_Specifications();
        public static double segments_Height { get; set; } = new double();
        public static double segments_Height_Next { get; set; } = new double();
        public static double cap_Height { get; set; } = new double();
        public static int start_Number { get; set; } = new int();
        public static string start_Nume { get; set; } = "";
        public static bool two_Shedulse { get; set; } = false;
        public static bool chekFormatLayersOne { get; set; } = false;
        public static ViewSheet lastlist { get; set; }
        public static double start_Size { get; set; } = 0;
        public static int sheet_number { get; set; } = 0;
        public static int shedule_Number { get; set; } = 0;
        public static ICollection<Element> shedule_Elements { get; set; } = new List<Element>();
        public static string shedule_Nume { get; set; } = "";
        public static string shedule_Nume_Next { get; set; } = "";
      
        public static ICollection<ViewSheet> sheets_Collection { get; set; } = new List<ViewSheet>();
        public static bool message_Duplicate { get; set; } = false;
        public static int sheet_9_Size { get; set; } = 0;
        public static string shedule_Elements_Result { get; set; } = "";
        public static string shedule_Elements_Result_Next { get; set; } = "";

        public static ICollection<string> stock_Element = new List<string>();
        public  static string name_Save_Element_Positon { get; set; } = "";
        public static ICollection<ViewSheet> sheet_Collection_Filtered { get; set; } = new List<ViewSheet>();
        public static ICollection<ViewSchedule> shedule_Equil { get; set; } = new List<ViewSchedule>();
        //Элементы основной надписи
        string[] shedule_Name_min = Data_Creating_Specifications.shedule_Nume.ToString().Split(new[] { "Тип:" }, StringSplitOptions.None);
        string[] shedule_Name_Next_min = Data_Creating_Specifications.shedule_Nume_Next.ToString().Split(new[] { "Тип:" }, StringSplitOptions.None);
        //if (Data_Creating_Specifications.chekFormatLayersOne == true && Data_Creating_Specifications.sheet_number != 0)
        //{
        //    shedule_Name_min = shedule_Name_Next_min;
        //}
        //Первая надпись
        public static string shedule_Name_Min_Last { get; set; }
        public static FilteredElementCollector collector { get; set; }
        public static Element titleBlock { get; set; }
        public static BoundingBoxXYZ bbx_Frame { get; set; }
        public static ElementId titleBlockid { get; set; } 
        public static double height_FrameX { get; set; } = 0;
        public static double height_FrameY { get; set; } = 0;
        public static bool list_Filter_Bool { get; set; } = true;
        public static bool segment_Filter_Bool { get; set; } = true;
        public static IList<SharedParameterDescriptor> list_Elements { get; set; } = new List<SharedParameterDescriptor>();
    }
    public static class Revit_Document_Creating_Specifications
    {
        public static UIApplication UIApplication { get; set;}
        public static UIDocument UIDobument { get => UIApplication.ActiveUIDocument; }
        public static Document Document { get => UIDobument.Document; }
        public static ExternalCommandData Data { get; set; }
        public static void Initialize(ExternalCommandData commandData)
        {
            Data = commandData;
            UIApplication = commandData.Application;
        }
    }
    public class Shedule_Save_Elements_List
    {
        public List<Shedule_Save_Elements> shedule_Save_Elements_List { get;set;} = new List<Shedule_Save_Elements>();
    }
    public class Shedule_Save_Elements
    {
        public string Name { get; set; }
        public List<string> shedule_Elements_Collection_Save { get; set; }
        public Shedule_Save_Elements() { }
        public Shedule_Save_Elements(string name,List<string> list_Elements)
        {
            Name = name;
            shedule_Elements_Collection_Save = list_Elements;
        }
    }
    public class list_Set_Elements
    {
        public ICollection<Shedule_Save_Elements> shedule_Save_Elements { get; set; } = new List<Shedule_Save_Elements>();
    }
}


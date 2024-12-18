using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WPFApplication.Assembling_Window
{
    public class Data_Assembling_On_Group_Stained_Glass_Windows
    {
        public static Guid guid_Group = new Guid("631cd69e-065f-4ec2-8894-4359325312c3");
        public static Guid guid_ADSK_Mark = new Guid("2204049c-d557-4dfc-8d70-13f19715e46d");
        public static List<Group_Assembly_Position> filtered_Group { get; set; } = new List<Group_Assembly_Position>();
        public static ObservableCollection<DataItem> DataItems { get; set; } = new ObservableCollection<DataItem>();
        public static List<ElementId> grup_Filtered_Collection { get; set; } = new List<ElementId>();
        public static int number_Assembly_Elements { get; set; } = 0;
        public static List<AssemblyDetailViewOrientation> AssemblyDetailViewOrientation { get; set; } = new List<AssemblyDetailViewOrientation>();
    }
    public static class Revit_Document_Assembling_On_Group_Stained_Glass_Windows
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
    public class DataItem : INotifyPropertyChanged
    {
        public DataItem() { }
        public string Mark { get; set; } = "";
        public string Type { get; set; } = "";
        public string Assembling { get; set; } = "";
        public string ID_Group { get; set; } = "";
        public string ID_Assembling { get; set; } = "";
        public bool Update { get; set; } = false;
        public DataItem(string Mark, string Type, string Assembling, string ID_Group, string ID_Assembling, bool Update)
        {
            this.Mark = Mark;
            this.Type = Type;
            this.Assembling = Assembling;
            this.ID_Group = ID_Group;
            this.ID_Assembling = ID_Assembling;
            this.Update = Update;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
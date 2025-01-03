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

namespace WPFApplication.Assembling_Door
{
    public class Data_Assembling_Window
    {
        public static Guid guid_Group = new Guid("631cd69e-065f-4ec2-8894-4359325312c3");
        public static Guid guid_ADSK_Mark = new Guid("2204049c-d557-4dfc-8d70-13f19715e46d");
        public static Guid guid_ADSK_NAME = new Guid("e6e0f5cd-3e26-485b-9342-23882b20eb43");
        public static Guid guid_COD = new Guid("631cd69e-065f-4ec2-8894-4359325312c3");
        public static List<Window_Assembly_Position> filtered_Group { get; set; } = new List<Window_Assembly_Position>();
        public static ObservableCollection<DataItem> DataItems { get; set; } = new ObservableCollection<DataItem>();
        public static List<ElementId> grup_Filtered_Collection { get; set; } = new List<ElementId>();
        public static int number_Assembly_Elements { get; set; } = 0;
        public static List<AssemblyDetailViewOrientation> AssemblyDetailViewOrientation { get; set; } = new List<AssemblyDetailViewOrientation>();
    }
    public static class Revit_Document_Assembling_Window
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
    public class Glass_Window
    {
        //Высота
        public string height { get; set; } = "";
        ///Ширина
        public string wight { get; set; } = "";
        //АТС_Тип_Изделия
        public string product_Type { get; set; } = "";
        //АТС_Материал_Профильных_Элементов
        public string material_Of_Profile_Elements { get; set; } = "";
        //АТС_Тип_Конструкции
        public string type_Of_Construction { get; set; } = "";
        //АТС_Формула_Стеклопакета
        public string double_Glazed_Window_Formula { get; set; } = null;
        //АТС_Спобос_Открывания
        public string opening_Method { get; set; } = "";
        //АТС_Покрытие_Окна_Спереди
        public string window_Covering_In_Front { get; set; } = "";
        //АТС_Покрытие_Окна_Сзади
        public string window_Covering_In_Back { get; set; } = "";
        //АТС_Огнестойкость
        public string fire_Resistance { get; set; } = "";
        //АТС_Утепленность
        public string insulation { get; set; } = "";
        //АТС_Функциональная_Особенность
        public string functional_Feature { get; set; } = "";
        //АТС_Расположение_Внутреннее_Наружное
        public string the_Location_Is_Internal { get; set; } = "";
        //АТС_Дополнительные_Сведенья
        public string additional_Information { get; set; } = "";
        public Element element_Window { get; set; } = null;
        public Glass_Window(
             string height,
             string wight,
             string product_Type,
             string material_Of_Profile_Elements,
             string type_Of_Construction,
             string double_Glazed_Window_Formula,
             string opening_Method,
             string window_Covering_In_Front,
             string window_Covering_In_Back,
             string fire_Resistance,
             string insulation,
             string functional_Feature,
             string the_Location_Is_Internal,
             string additional_Information,
             Element element_Window)
        {
        this.height = height;
        this.wight = wight;
        this.product_Type = product_Type;
        this.material_Of_Profile_Elements = material_Of_Profile_Elements;
        this.type_Of_Construction = type_Of_Construction;
        this.double_Glazed_Window_Formula = double_Glazed_Window_Formula;
        this.opening_Method = opening_Method;
        this.fire_Resistance = fire_Resistance;
        this.insulation = insulation;
        this.functional_Feature = functional_Feature;
        this.the_Location_Is_Internal = the_Location_Is_Internal;
        this.additional_Information = additional_Information;
        this.element_Window = element_Window;
        }
    }
}
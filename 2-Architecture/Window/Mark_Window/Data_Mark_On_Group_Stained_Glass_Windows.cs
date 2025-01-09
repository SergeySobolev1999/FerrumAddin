using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPFApplication.Parameter_Window;

namespace WPFApplication.Mark_Window
{
    public class Data_Mark_Window
    {
        public static List<Element> filtered_Group { get; set; } = new List<Element>();
        public static Guid guid_Group = new Guid("631cd69e-065f-4ec2-8894-4359325312c3");
        public static Guid guid_ADSK_Mark = new Guid("2204049c-d557-4dfc-8d70-13f19715e46d");
        public static Guid guid_ADSK_NAME = new Guid("e6e0f5cd-3e26-485b-9342-23882b20eb43");
        public static List<Glass_Window> list_Group { get; set; } = new List<Glass_Window>();
        public static Guid guid_COD = new Guid("631cd69e-065f-4ec2-8894-4359325312c3");
        public static int number_Elements { get; set; } = 0;
        public static int number_Elements_Transom { get; set; } = 0;
        public static bool iteration_Recaive_Value_In_Parameter = false;

        public static string  iteration_Recaive_Value_In_Parameter_Watringn = "";

    }
    public static class Revit_Document_Mark_Window 
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
        public string double_Glazed_Window_Formula { get; set; } = "";
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

        //АТС_Класс_Сопротивления_Теплопередаче
        public string heat_Transfer_Resistance_Class { get; set; } = "";
        //АТС_Формула_Стеклопакета
        public string the_Double_Glazed_Unit_Formula { get; set; } = "";
        //АТС_Расположение_Внутреннее_Наружное
        public string location_Indoor_Outdoor { get; set; } = "";
        //Элемент
        public Element element_Window { get; set; } = null;
        //АТС_Материал_Рамочных_Элементов
        public string the_Material_Of_The_Frame_Elements { get; set; } = "";
        public Glass_Window(
            Element element_Window,
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
            string additional_Information
            )
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
        public Glass_Window(
            string height,
            Element element_Window,
            string wight,
            string product_Type,
            string heat_Transfer_Resistance_Class,
            string the_Double_Glazed_Unit_Formula,
            string fire_Resistance,
            string window_Covering_In_Front,
            string window_Covering_In_Back,
            string insulation,
            string functional_Feature,
            string location_Indoor_Outdoor,
            string additional_Information)
        {
            this.height = height;
            this.wight = wight;
            this.product_Type = product_Type;
            this.heat_Transfer_Resistance_Class = heat_Transfer_Resistance_Class;
            this.the_Double_Glazed_Unit_Formula = the_Double_Glazed_Unit_Formula;
            this.fire_Resistance = fire_Resistance;
            this.window_Covering_In_Front = window_Covering_In_Front;
            this.window_Covering_In_Back = window_Covering_In_Back;
            this.insulation = insulation;
            this.functional_Feature = functional_Feature;
            this.location_Indoor_Outdoor = location_Indoor_Outdoor;
            this.additional_Information = additional_Information;
            this.element_Window = element_Window;
        }
        public Glass_Window(
         string height,
         string wight,
         Element element_Window,
         string the_Material_Of_The_Frame_Elements,
         string the_Double_Glazed_Unit_Formula,
         string fire_Resistance,
         string window_Covering_In_Front,
         string window_Covering_In_Back,
         string insulation,
         string functional_Feature,
         string location_Indoor_Outdoor,
         string additional_Information
        )
        {
            this.height = height;
            this.wight = wight;
            this.the_Material_Of_The_Frame_Elements = the_Material_Of_The_Frame_Elements;
            this.the_Double_Glazed_Unit_Formula = the_Double_Glazed_Unit_Formula;
            this.fire_Resistance = fire_Resistance;
            this.window_Covering_In_Front = window_Covering_In_Front;
            this.window_Covering_In_Back = window_Covering_In_Back;
            this.insulation = insulation;
            this.functional_Feature = functional_Feature;
            this.location_Indoor_Outdoor = location_Indoor_Outdoor;
            this.additional_Information = additional_Information;
            this.element_Window = element_Window;
        }

    }
}

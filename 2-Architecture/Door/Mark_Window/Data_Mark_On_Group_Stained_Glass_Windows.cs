using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPFApplication.Parameter_Window;
using System.Globalization;
using System.Windows.Media.Media3D;

namespace WPFApplication.Mark_Door
{
    public class Data_Mark_Door
    {
        public static List<Element> filtered_Group { get; set; } = new List<Element>();
        public static Guid guid_Group = new Guid("631cd69e-065f-4ec2-8894-4359325312c3");
        public static Guid guid_ADSK_Mark = new Guid("2204049c-d557-4dfc-8d70-13f19715e46d");
        public static Guid guid_ADSK_NAME = new Guid("e6e0f5cd-3e26-485b-9342-23882b20eb43");
        public static List<Glass_Window> list_Group { get; set; } = new List<Glass_Window>();
        public static Guid guid_COD = new Guid("631cd69e-065f-4ec2-8894-4359325312c3");
        public static int number_Elements { get; set; } = 0;
        public static int number_Elements_Balcony { get; set; } = 0;
        public static int number_Elements_Gates { get; set; } = 0;
        public static bool iteration_Recaive_Value_In_Parameter = false;
        public static bool error_Suppressio = false;
        public static int number_Elements_Transom { get; set; } = 0;
        public static string iteration_Recaive_Value_In_Parameter_Watringn = "";

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
        //АТС_Тип_Изделия
        public string product_Type { get; set; } = "";
        //АТС_Материал_Конструкции
        public string material_Constructions { get; set; } = "";
        //АТС_Назначение_Изделия
        public string application_Production { get; set; } = "";
        //АТС_Способ_Открывания
        public string performance_Openings { get; set; } = "";
        //АТС_Конструктивное_Исполнение
        public string material_Of_Profile_Elements { get; set; } = "";
        //АТС_Порог
        public string doorstep { get; set; } = "";
        //АТС_Открывание_Лево_Право
        public string double_Glazed_Window_Formula { get; set; } = "";
        //АТС_Спобос_Открывания
        public string opening_Method { get; set; } = "";
        //АТС_Вид_Заполнения
        public string type_Of_Filling { get; set; } = "";
        //АТС_Открывание_Внутрь_Наружу
        public string opening_Inside_To_Outside { get; set; } = "";
        //Высота
        public string height { get; set; } = "";
        //Ширина
        public string wight { get; set; } = "";
        //АТС_Огнестойкость
        public string fire_Resistance { get; set; } = "";
        //АТС_Утепленность
        public string insulation { get; set; } = "";
        //АТС_Функциональная_Особенность
        public string functional_Feature { get; set; } = "";
        //АТС_Покрытие_Окна_Спереди
        public string window_Covering_In_Front { get; set; } = "";
        //АТС_Покрытие_Окна_Сзади
        public string window_Covering_In_Back { get; set; } = "";
        //АТС_Дополнительные_Сведенья
        public string additional_Information { get; set; } = "";
        //Элемент
        public Element element_Doors { get; set; } = null;
        //АТС_Материал_Рамочных_Элементов
        public string the_Material_Of_The_Frame_Elements { get; set; } = "";
        //АТС_Назначение_Изделия
        public string purpose_Of_The_Product { get; set; } = "";
        //АТС_Конструктивное_Исполнение
        public string heat_Transfer_Resistance_Class { get; set; } = "";
        //АТС_Открывание_Лево_Право
        public string opening_Left_Right { get; set; } = "";
        //АТС_Расположение_Внутреннее_Наружное
        public string location_Indoor_Outdoor { get; set; } = "";
        //АТС_Исполнение_Двери
        public string door_Execution { get; set; } = "";
        //АТС_Класс_По_Эксплуатационным_Характеристикам
        public string operational_Pperformance_Class { get; set; } = "";
        //АТС_Класс_Прочности
        public string strength_Сlass { get; set; } = "";
        //АТС_Дополнительное_Обозначение_Исполнения
        public string additional_Designation_Of_The_Execution { get; set; } = "";
        //АТС_Число_Полотен
        public string number_Of_Canvases { get; set; } = "";
        //БТС_Наличие_Остекления_Переопределить
        public string the_Presence_Of_Glazing { get; set; } = "";
        //АТС_Формула_Стеклопакета
        public string the_Double_Glazed_Unit_Formula { get; set; } = "";
        public Glass_Window(
            Element element_Doors,
            string product_Type,
            string material_Of_Profile_Elements,
            string doorstep,
            string double_Glazed_Window_Formula,
            string opening_Method,
            string type_Of_Filling,
            string щpening_Inside_To_Outside,
            string height,
            string wight,
            string fire_Resistance,
            string insulation,
            string functional_Feature,
            string window_Covering_In_Front,
            string window_Covering_In_Back,
            string additional_Information
            )
        {
            this.product_Type = product_Type;
            this.material_Of_Profile_Elements = material_Of_Profile_Elements;
            this.doorstep = doorstep;
            this.double_Glazed_Window_Formula = double_Glazed_Window_Formula;
            this.element_Doors = element_Doors;
            this.opening_Method = opening_Method;
            this.type_Of_Filling = type_Of_Filling;
            this.opening_Inside_To_Outside = щpening_Inside_To_Outside;
            this.height = height;
            this.wight = wight;
            this.fire_Resistance = fire_Resistance;
            this.insulation = insulation;
            this.functional_Feature = functional_Feature;
            this.window_Covering_In_Front = window_Covering_In_Front;
            this.insulation = insulation;
            this.functional_Feature = functional_Feature;
        }
        public Glass_Window(
            string purpose_Of_The_Product,
            Element element_Doors,
            string heat_Transfer_Resistance_Class,
            string opening_Left_Right,
            string щpening_Inside_To_Outside,
            string height,
            string wight,
            string fire_Resistance,
            string insulation,
            string functional_Feature,
            string location_Indoor_Outdoor,
            string window_Covering_In_Front,
            string window_Covering_In_Back,
            string additional_Information
            )
        {
            this.purpose_Of_The_Product = purpose_Of_The_Product;
            this.element_Doors = element_Doors;
            this.heat_Transfer_Resistance_Class = heat_Transfer_Resistance_Class;
            this.opening_Left_Right = opening_Left_Right;
            this.opening_Inside_To_Outside = щpening_Inside_To_Outside;
            this.height = height;
            this.wight = wight;
            this.fire_Resistance = fire_Resistance;
            this.insulation = insulation;
            this.functional_Feature = functional_Feature;
            this.location_Indoor_Outdoor = location_Indoor_Outdoor;
            this.window_Covering_In_Front = window_Covering_In_Front;
            this.window_Covering_In_Back = window_Covering_In_Back;
            this.additional_Information = additional_Information;
        }
        public Glass_Window(
            string the_Material_Of_The_Frame_Elements,
            string heat_Transfer_Resistance_Class,
            Element element_Doors,
            string the_Double_Glazed_Unit_Formula,
            string opening_Left_Right,
            string щpening_Inside_To_Outside,
            string door_Execution,
            string operational_Pperformance_Class,
            string strength_Сlass,
            string additional_Designation_Of_The_Execution,
            string height,
            string wight,
            string fire_Resistance,
            string insulation,
            string functional_Feature,
            string location_Indoor_Outdoor,
            string window_Covering_In_Front,
            string window_Covering_In_Back,
            string additional_Information
            )
        {
            this.the_Material_Of_The_Frame_Elements = the_Material_Of_The_Frame_Elements;
            this.heat_Transfer_Resistance_Class = heat_Transfer_Resistance_Class;
            this.element_Doors = element_Doors;
            this.doorstep = doorstep;
            this.opening_Left_Right = opening_Left_Right;
            this.opening_Inside_To_Outside = щpening_Inside_To_Outside;
            this.door_Execution = door_Execution;
            this.operational_Pperformance_Class = operational_Pperformance_Class;
            this.strength_Сlass = strength_Сlass;
            this.additional_Designation_Of_The_Execution = additional_Designation_Of_The_Execution;
            this.height = height;
            this.wight = wight;
            this.fire_Resistance = fire_Resistance;
            this.insulation = insulation;
            this.functional_Feature = functional_Feature;
            this.location_Indoor_Outdoor = location_Indoor_Outdoor;
            this.window_Covering_In_Front = window_Covering_In_Front;
            this.window_Covering_In_Back = window_Covering_In_Back;
            this.additional_Information = additional_Information;
        }
        public Glass_Window(
           string the_Material_Of_The_Frame_Elements,
           string heat_Transfer_Resistance_Class,
           string number_Of_Canvases,
           Element element_Doors,
           string the_Presence_Of_Glazing,
           string doorstep,
           string щpening_Inside_To_Outside,
           string height,
           string wight,
           string fire_Resistance,
           string insulation,
           string functional_Feature,
           string window_Covering_In_Front,
           string window_Covering_In_Back,
           string additional_Information
           )
        {
            this.the_Material_Of_The_Frame_Elements = the_Material_Of_The_Frame_Elements;
            this.heat_Transfer_Resistance_Class = heat_Transfer_Resistance_Class;
            this.number_Of_Canvases = number_Of_Canvases;
            this.element_Doors = element_Doors;
            this.the_Presence_Of_Glazing = the_Presence_Of_Glazing;
            this.doorstep = doorstep;
            this.opening_Inside_To_Outside = щpening_Inside_To_Outside;
            this.height = height;
            this.wight = wight;
            this.fire_Resistance = fire_Resistance;
            this.insulation = insulation;
            this.functional_Feature = functional_Feature;
            this.location_Indoor_Outdoor = location_Indoor_Outdoor;
            this.window_Covering_In_Front = window_Covering_In_Front;
            this.window_Covering_In_Back = window_Covering_In_Back;
            this.additional_Information = additional_Information;
        }
        public Glass_Window(
          string product_Type,
          string heat_Transfer_Resistance_Class,
          string height,
          string wight,
          Element element_Doors,
          string number_Of_Canvases,
          string щpening_Inside_To_Outside,
          string opening_Left_Right,
          string the_Double_Glazed_Unit_Formula,
          string fire_Resistance,
          string insulation,
          string functional_Feature,
          string window_Covering_In_Front,
          string window_Covering_In_Back,
          string additional_Information
          )
        {
            this.product_Type = product_Type;
            this.heat_Transfer_Resistance_Class = heat_Transfer_Resistance_Class;
            this.height = height;
            this.wight = wight;
            this.element_Doors = element_Doors;
            this.number_Of_Canvases = number_Of_Canvases;
            this.opening_Inside_To_Outside = щpening_Inside_To_Outside;
            this.opening_Left_Right = opening_Left_Right;
            this.the_Double_Glazed_Unit_Formula = the_Double_Glazed_Unit_Formula;
            this.fire_Resistance = fire_Resistance;
            this.insulation = insulation;
            this.functional_Feature = functional_Feature;
            this.location_Indoor_Outdoor = location_Indoor_Outdoor;
            this.window_Covering_In_Front = window_Covering_In_Front;
            this.window_Covering_In_Back = window_Covering_In_Back;
            this.additional_Information = additional_Information;
        }
        public Glass_Window(
            string product_Type,
            string material_Of_Profile_Elements,
            string doorstep,
            string double_Glazed_Window_Formula,
            Element element_Doors,
            string opening_Method,
            string type_Of_Filling,
            string opening_Inside_To_Outside,
            string height,
            string wight,
            string fire_Resistance,
            string insulation,
            string functional_Feature,
            string window_Covering_In_Front,
            string window_Covering_In_Back,
            string additional_Information
            )
        {
            this.product_Type = product_Type;
            this.material_Of_Profile_Elements = material_Of_Profile_Elements;
            this.doorstep = doorstep;
            this.double_Glazed_Window_Formula = double_Glazed_Window_Formula;
            this.element_Doors = element_Doors;
            this.opening_Method = opening_Method;
            this.type_Of_Filling = type_Of_Filling;
            this.opening_Inside_To_Outside = opening_Inside_To_Outside;
            this.height = height;
            this.wight = wight;
            this.fire_Resistance = fire_Resistance;
            this.insulation = insulation;
            this.functional_Feature = functional_Feature;
            this.window_Covering_In_Front = window_Covering_In_Front;
            this.window_Covering_In_Back = window_Covering_In_Back;
            this.additional_Information = additional_Information;
        }
        public Glass_Window(
           string heat_Transfer_Resistance_Class,
           string doorstep,
           string opening_Left_Right,
           string opening_Inside_To_Outside,
           string height,
           Element element_Doors,
           string wight,
           string fire_Resistance,
           string insulation,
           string functional_Feature,
           string location_Indoor_Outdoor,
           string window_Covering_In_Front,
           string window_Covering_In_Back,
           string additional_Information
           )
        {
            this.heat_Transfer_Resistance_Class = heat_Transfer_Resistance_Class;
            this.doorstep = doorstep;
            this.opening_Left_Right = opening_Left_Right;
            this.opening_Inside_To_Outside = opening_Inside_To_Outside;
            this.height = height;
            this.element_Doors = element_Doors;
            this.wight = wight;
            this.fire_Resistance = fire_Resistance;
            this.insulation = insulation;
            this.functional_Feature = functional_Feature;
            this.location_Indoor_Outdoor = location_Indoor_Outdoor;
            this.window_Covering_In_Front = window_Covering_In_Front;
            this.window_Covering_In_Back = window_Covering_In_Back;
            this.additional_Information = additional_Information;
        }
        public Glass_Window(
           string material_Constructions,
           string application_Production,
           string performance_Openings,
           string opening_Inside_To_Outside,
           string height,
           string wight,
           Element element_Doors,
           string fire_Resistance,
           string insulation,
           string functional_Feature,
           string additional_Information
           )
        {
            this.material_Constructions = material_Constructions;
            this.application_Production = application_Production;
            this.performance_Openings = performance_Openings;
            this.opening_Inside_To_Outside = opening_Inside_To_Outside;
            this.height = height;
            this.wight = wight;
            this.element_Doors = element_Doors;
            this.fire_Resistance = fire_Resistance;
            this.insulation = insulation;
            this.functional_Feature = functional_Feature;
            this.additional_Information = additional_Information;
        }
    }
}
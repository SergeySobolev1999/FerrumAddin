using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.UI.Selection;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using WPFApplication.Parameter_On_Group_Stained_Glass_Windows;
using SSDK;
using System.Windows.Controls;
using System.Globalization;
using System.Windows.Media.Media3D;

namespace WPFApplication.Mark_Door
{
    public class Collecting_Window
    {
        public Collecting_Window()
        {
            try
            {
                using (Transaction newT1 = new Transaction(Revit_Document_Mark_Window.Document, "Выгрузка данных формата "))
                {
                    newT1.Start();
                    if (Data_Mark_Door.error_Suppressio == true)
                    {
                        // Настройка для подавления предупреждений
                        FailureHandlingOptions failureOptions = newT1.GetFailureHandlingOptions();
                        failureOptions.SetFailuresPreprocessor(new IgnoreWarningPreprocessor());
                        newT1.SetFailureHandlingOptions(failureOptions);
                    }
                    if (Data_Mark_Door.filtered_Group.Count > 0)
                    {
                        foreach (Element element_Familyinstace in Data_Mark_Door.filtered_Group)
                        {
                            Element element_Group_Ex = Revit_Document_Mark_Window.Document.GetElement(element_Familyinstace.Id);
                            Element element_Doors = Revit_Document_Mark_Window.Document.GetElement(element_Familyinstace.GetTypeId());

                            double parameter_Value = element_Doors.get_Parameter(Data_Mark_Door.guid_COD).AsDouble() * 304.8;
                         
                                Parameter_Name parameter_Name = new Parameter_Name();
                                if (element_Doors.LookupParameter("ЮТС_Dynamo_ID") != null)
                                {
                                    if (element_Doors != null && element_Doors.LookupParameter("ЮТС_Dynamo_ID").AsValueString() == "ГОСТ_23747_Д_Новое")
                                    {
                                        //АТС_Тип_Изделия
                                        string product_Type = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Назначение_Изделия", "БТС_Назначение_Изделия_Переопределить");
                                        //АТС_Конструктивное_Исполнение
                                        string material_Of_Profile_Elements = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Конструктивное_Исполнение", "БТС_Конструктивное_Исполнение_Переопределить");
                                        //АТС_Порог
                                        string doorstep = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Порог", "БТС_Порог_Переопределить");
                                        //АТС_Открывание_Лево_Право
                                        string double_Glazed_Window_Formula = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Открывание_Лево_Право", "БТС_Открывание_Лево_Право_Переопределить");
                                        //АТС_Спобос_Открывания
                                        string opening_Method = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Способ_Открывания", "БТС_Способ_Открывания_Переопределить");
                                        //АТС_Вид_Заполнения
                                        string type_Of_Filling = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Вид_Заполнения", "БТС_Вид_Заполнения_Переопределить");
                                        //АТС_Открывание_Внутрь_Наружу
                                        string щpening_Inside_To_Outside = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Открывание_Внутрь_Наружу", "БТС_Открывание_Внутрь_Наружу_Переопределить");
                                        //Высота
                                        string height = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТР_Примерная_Высота", "-"), CultureInfo.InvariantCulture))).ToString();
                                        //Ширина
                                        string wight = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТР_Примерная_Ширина", "-"), CultureInfo.InvariantCulture))).ToString();
                                        //АТС_Огнестойкость
                                        string fire_Resistance = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Огнестойкость", "БТС_Огнестойкость_Переопределить");
                                        //АТС_Утепленность
                                        string insulation = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Утепленность", "БТС_Утепленность_Переопределить");
                                        //АТС_Функциональная_Особенность
                                        string functional_Feature = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Функциональная_Особенность", "БТС_Функциональная_Особенность_Переопределить");
                                        //АТС_Покрытие_Окна_Спереди
                                        string window_Covering_In_Front = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Покрытие_Двери_Спереди", "-");
                                        //АТС_Покрытие_Окна_Сзади
                                        string window_Covering_In_Back = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Покрытие_Двери_Сзади", "-"); ;
                                        //АТС_Дополнительные_Сведенья
                                        string additional_Information = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Дополнительные_Сведенья", "-");
                                        Glass_Window glass_Window = new Glass_Window(
                                            element_Doors,
                                            product_Type,
                                            material_Of_Profile_Elements,
                                            doorstep,
                                            double_Glazed_Window_Formula,
                                            opening_Method,
                                            type_Of_Filling,
                                            щpening_Inside_To_Outside,
                                            height,
                                            wight,
                                            fire_Resistance,
                                            insulation,
                                            functional_Feature,
                                            window_Covering_In_Front,
                                            window_Covering_In_Back,
                                            additional_Information);
                                        Data_Mark_Door.list_Group.Add(glass_Window);
                                    }
                                    if (element_Doors != null && element_Doors.LookupParameter("ЮТС_Dynamo_ID").AsValueString() == "ГОСТ_57327_Д_Новое")
                                    {
                                        //АТС_Назначение_Изделия
                                        string purpose_Of_The_Product = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Назначение_Изделия", "БТС_Назначение_Изделия_Переопределить");
                                        //АТС_Конструктивное_Исполнение
                                        string heat_Transfer_Resistance_Class = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Конструктивное_Исполнение", "БТС_Конструктивное_Исполнение_Переопределить");
                                        //АТС_Открывание_Лево_Право
                                        string opening_Left_Right = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Открывание_Лево_Право", "БТС_Открывание_Лево_Право_Переопределить");
                                        //АТС_Открывание_Внутрь_Наружу
                                        string щpening_Inside_To_Outside = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Открывание_Внутрь_Наружу", "БТС_Открывание_Внутрь_Наружу_Переопределить");
                                        //Высота
                                        string height = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТР_Примерная_Высота", "-"), CultureInfo.InvariantCulture))).ToString();
                                        //Ширина
                                        string wight = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТР_Примерная_Ширина", "-"), CultureInfo.InvariantCulture))).ToString();
                                        //АТС_Огнестойкость
                                        string fire_Resistance = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Огнестойкость", "БТС_Огнестойкость_Переопределить");
                                        //АТС_Утепленность
                                        string insulation = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Утепленность", "БТС_Утепленность_Переопределить");
                                        //АТС_Функциональная_Особенность
                                        string functional_Feature = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Функциональная_Особенность", "БТС_Функциональная_Особенность_Переопределить");
                                        //АТС_Расположение_Внутреннее_Наружное
                                        string location_Indoor_Outdoor = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Расположение_Внутреннее_Наружное", "-");
                                        //АТС_Покрытие_Окна_Спереди
                                        string window_Covering_In_Front = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Покрытие_Двери_Спереди", "-");
                                        //АТС_Покрытие_Окна_Сзади
                                        string window_Covering_In_Back = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Покрытие_Двери_Сзади", "-"); ;
                                        //АТС_Дополнительные_Сведенья
                                        string additional_Information = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Дополнительные_Сведенья", "-");
                                        Glass_Window glass_Window = new Glass_Window(
                                            purpose_Of_The_Product,
                                            element_Doors,
                                            heat_Transfer_Resistance_Class,
                                            opening_Left_Right,
                                            щpening_Inside_To_Outside,
                                            height,
                                            wight,
                                            fire_Resistance,
                                            insulation,
                                            functional_Feature,
                                            location_Indoor_Outdoor,
                                            window_Covering_In_Front,
                                            window_Covering_In_Back,
                                            additional_Information);
                                        Data_Mark_Door.list_Group.Add(glass_Window);
                                    }
                                    if (element_Doors != null && element_Doors.LookupParameter("ЮТС_Dynamo_ID").AsValueString() == "ГОСТ_31173_Д_Новое")
                                    {
                                        //АТС_Назначение_Изделия
                                        string the_Material_Of_The_Frame_Elements = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Назначение_Изделия", "БТС_Назначение_Изделия_Переопределить");
                                        //АТС_Конструктивное_Исполнение
                                        string heat_Transfer_Resistance_Class = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Конструктивное_Исполнение", "БТС_Конструктивное_Исполнение_Переопределить");
                                        //АТС_Порог
                                        string doorstep = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Порог", "БТС_Порог_Переопределить");
                                        //АТС_Открывание_Лево_Право
                                        string opening_Left_Right = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Открывание_Лево_Право", "БТС_Открывание_Лево_Право_Переопределить");
                                        //АТС_Открывание_Внутрь_Наружу
                                        string щpening_Inside_To_Outside = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Открывание_Внутрь_Наружу", "БТС_Открывание_Внутрь_Наружу_Переопределить");
                                        //АТС_Исполнение_Двери
                                        string door_Execution = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Исполнение_Двери", "БТС_Исполнение_Двери_Переопределить");
                                        //АТС_Класс_По_Эксплуатационным_Характеристикам
                                        string operational_Pperformance_Class = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Класс_По_Эксплуатационным_Характеристикам", "БТС_Класс_По_Эксплуатационным_Характеристикам");
                                        //АТС_Класс_Прочности
                                        string strength_Сlass = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Класс_Прочности", "БТС_Класс_Прочности_Переопределить");
                                        //АТС_Дополнительное_Обозначение_Исполнения
                                        string additional_Designation_Of_The_Execution = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Дополнительное_Обозначение_Исполнения", "БТС_Дополнительное_Обозначение_Исполнения_Переопределить");
                                        //Высота
                                        string height = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТР_Примерная_Высота", "-"), CultureInfo.InvariantCulture))).ToString();
                                        //Ширина
                                        string wight = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТР_Примерная_Ширина", "-"), CultureInfo.InvariantCulture))).ToString();
                                        //АТС_Огнестойкость
                                        string fire_Resistance = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Огнестойкость", "БТС_Огнестойкость_Переопределить");
                                        //АТС_Утепленность
                                        string insulation = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Утепленность", "БТС_Утепленность_Переопределить");
                                        //АТС_Функциональная_Особенность
                                        string functional_Feature = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Функциональная_Особенность", "БТС_Функциональная_Особенность_Переопределить");
                                        //АТС_Расположение_Внутреннее_Наружное
                                        string location_Indoor_Outdoor = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Расположение_Внутреннее_Наружное", "-");
                                        //АТС_Покрытие_Окна_Спереди
                                        string window_Covering_In_Front = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Покрытие_Двери_Спереди", "-");
                                        //АТС_Покрытие_Окна_Сзади
                                        string window_Covering_In_Back = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Покрытие_Двери_Сзади", "-");
                                        //АТС_Дополнительные_Сведенья
                                        string additional_Information = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Дополнительные_Сведенья", "-");
                                        Glass_Window glass_Window = new Glass_Window(
                                            the_Material_Of_The_Frame_Elements,
                                            heat_Transfer_Resistance_Class,
                                            element_Doors,
                                            doorstep,
                                            opening_Left_Right,
                                            щpening_Inside_To_Outside,
                                            door_Execution,
                                            operational_Pperformance_Class,
                                            strength_Сlass,
                                            additional_Designation_Of_The_Execution,
                                            height,
                                            wight,
                                            fire_Resistance,
                                            insulation,
                                            functional_Feature,
                                            location_Indoor_Outdoor,
                                            window_Covering_In_Front,
                                            window_Covering_In_Back,
                                            additional_Information);
                                        Data_Mark_Door.list_Group.Add(glass_Window);
                                    }
                                    if (element_Doors != null && element_Doors.LookupParameter("ЮТС_Dynamo_ID").AsValueString() == "ГОСТ_475_Д_Новое")
                                    {
                                        //АТС_Назначение_Изделия
                                        string the_Material_Of_The_Frame_Elements = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Назначение_Изделия", "БТС_Назначение_Изделия_Переопределить");
                                        //АТС_Направление_И_Способ_Открывания
                                        string heat_Transfer_Resistance_Class = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Направление_И_Способ_Открывания", "БТС_Направление_И_Способ_Открывания_Переопределить");
                                        //АТС_Число_Полотен
                                        string number_Of_Canvases = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Число_Полотен", "БТС_Число_Полотен_Переопределить");
                                        //БТС_Наличие_Остекления_Переопределить
                                        string the_Presence_Of_Glazing = parameter_Name.Parameter_Name_Of_Element(element_Doors, "БТС_Наличие_Остекления_Переопределить", "БТС_Наличие_Остекления_Переопределить");
                                        //АТС_Порог
                                        string doorstep = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Порог", "БТС_Порог_Переопределить");
                                        //АТС_Открывание_Внутрь_Наружу
                                        string щpening_Inside_To_Outside = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Открывание_Внутрь_Наружу", "БТС_Открывание_Внутрь_Наружу_Переопределить");
                                        //Высота
                                        string height = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТР_Примерная_Высота", "-"), CultureInfo.InvariantCulture))).ToString();
                                        //Ширина
                                        string wight = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТР_Примерная_Ширина", "-"), CultureInfo.InvariantCulture))).ToString();
                                        //АТС_Огнестойкость
                                        string fire_Resistance = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Огнестойкость", "БТС_Огнестойкость_Переопределить");
                                        //АТС_Утепленность
                                        string insulation = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Утепленность", "БТС_Утепленность_Переопределить");
                                        //АТС_Функциональная_Особенность
                                        string functional_Feature = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Функциональная_Особенность", "БТС_Функциональная_Особенность_Переопределить");
                                        //АТС_Покрытие_Окна_Спереди
                                        string window_Covering_In_Front = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Покрытие_Двери_Спереди", "-");
                                        //АТС_Покрытие_Окна_Сзади
                                        string window_Covering_In_Back = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Покрытие_Двери_Сзади", "-");
                                        //АТС_Дополнительные_Сведенья
                                        string additional_Information = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Дополнительные_Сведенья", "-");
                                        Glass_Window glass_Window = new Glass_Window(
                                            the_Material_Of_The_Frame_Elements,
                                            heat_Transfer_Resistance_Class,
                                            number_Of_Canvases,
                                            element_Doors,
                                            the_Presence_Of_Glazing,
                                            doorstep,
                                            щpening_Inside_To_Outside,
                                            height,
                                            wight,
                                            fire_Resistance,
                                            insulation,
                                            functional_Feature,
                                            window_Covering_In_Front,
                                            window_Covering_In_Back,
                                            additional_Information);
                                        Data_Mark_Door.list_Group.Add(glass_Window);
                                    }
                                    if (element_Doors != null && (element_Doors.LookupParameter("ЮТС_Dynamo_ID").AsValueString() == "ГОСТ_30674_ДБ_Новое" || element_Doors.LookupParameter("ЮТС_Dynamo_ID").AsValueString() == "ГОСТ_30674_Д_Новое"))
                                    {
                                        //АТС_Вид_Изделия
                                        string product_Type = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Вид_Изделия", "БТС_Вид_Изделия_Переопределить");
                                        //АТС_Класс_Сопротивления_Теплопередаче
                                        string heat_Transfer_Resistance_Class = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Класс_Сопротивления_Теплопередаче", "БТС_Класс_Сопротивления_Теплопередаче_Переопределить");
                                        //Высота
                                        string height = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТР_Примерная_Высота", "-"), CultureInfo.InvariantCulture))).ToString();
                                        //Ширина
                                        string wight = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТР_Примерная_Ширина", "-"), CultureInfo.InvariantCulture))).ToString();
                                        //АТС_Конструктивное_Исполнение
                                        string number_Of_Canvases = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Конструктивное_Исполнение", "БТС_Конструктивное_Исполнение_Переопределить");
                                        //АТС_Открывание_Внутрь_Наружу
                                        string щpening_Inside_To_Outside = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Открывание_Внутрь_Наружу", "БТС_Открывание_Внутрь_Наружу_Переопределить");
                                        //АТС_Открывание_Лево_Право
                                        string opening_Left_Right = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Открывание_Лево_Право", "БТС_Открывание_Лево_Право_Переопределить");
                                        //АТС_Формула_Стеклопакета
                                        string the_Double_Glazed_Unit_Formula = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Формула_Стеклопакета", "БТС_Формула_Стеклопакета_Переопределить");
                                        //АТС_Огнестойкость
                                        string fire_Resistance = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Огнестойкость", "БТС_Огнестойкость_Переопределить");
                                        //АТС_Утепленность
                                        string insulation = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Утепленность", "БТС_Утепленность_Переопределить");
                                        //АТС_Функциональная_Особенность
                                        string functional_Feature = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Функциональная_Особенность", "БТС_Функциональная_Особенность_Переопределить");
                                        //АТС_Покрытие_Окна_Спереди
                                        string window_Covering_In_Front = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Покрытие_Двери_Спереди", "-");
                                        //АТС_Покрытие_Окна_Сзади
                                        string window_Covering_In_Back = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Покрытие_Двери_Сзади", "-");
                                        //АТС_Дополнительные_Сведенья
                                        string additional_Information = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Дополнительные_Сведенья", "-");
                                        Glass_Window glass_Window = new Glass_Window(
                                            product_Type,
                                            heat_Transfer_Resistance_Class,
                                            height,
                                            wight,
                                            element_Doors,
                                            number_Of_Canvases,
                                            щpening_Inside_To_Outside,
                                            opening_Left_Right,
                                            the_Double_Glazed_Unit_Formula,
                                            fire_Resistance,
                                            insulation,
                                            functional_Feature,
                                            window_Covering_In_Front,
                                            window_Covering_In_Back,
                                            additional_Information);
                                        Data_Mark_Door.list_Group.Add(glass_Window);
                                    }
                                    if (element_Doors != null && element_Doors.LookupParameter("ЮТС_Dynamo_ID").AsValueString() == "ГОСТ_23747_Д_Витражная_Новое")
                                    {
                                        //АТС_Тип_Изделия
                                        string product_Type = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Назначение_Изделия", "БТС_Назначение_Изделия_Переопределить");
                                        //АТС_Конструктивное_Исполнение
                                        string material_Of_Profile_Elements = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Конструктивное_Исполнение", "БТС_Конструктивное_Исполнение_Переопределить");
                                        //АТС_Порог
                                        string doorstep = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Порог", "БТС_Порог_Переопределить");
                                        //АТС_Открывание_Лево_Право
                                        string double_Glazed_Window_Formula = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Открывание_Лево_Право", "БТС_Открывание_Лево_Право_Переопределить");
                                        //АТС_Спобос_Открывания
                                        string opening_Method = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Способ_Открывания", "БТС_Способ_Открывания_Переопределить");
                                        //АТС_Вид_Заполнения
                                        string type_Of_Filling = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Вид_Заполнения", "БТС_Вид_Заполнения_Переопределить");
                                        //АТС_Открывание_Внутрь_Наружу
                                        string щpening_Inside_To_Outside = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Открывание_Внутрь_Наружу", "БТС_Открывание_Внутрь_Наружу_Переопределить");
                                        //Высота
                                        string height = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Group_Ex, "АЭР_Примерная_Высота", "-"), CultureInfo.InvariantCulture))).ToString();
                                        //Ширина
                                        string wight = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Group_Ex, "АЭР_Примерная_Ширина", "-"), CultureInfo.InvariantCulture))).ToString();
                                        //АТС_Огнестойкость
                                        string fire_Resistance = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Огнестойкость", "БТС_Огнестойкость_Переопределить");
                                        //АТС_Утепленность
                                        string insulation = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Утепленность", "БТС_Утепленность_Переопределить");
                                        //АТС_Функциональная_Особенность
                                        string functional_Feature = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Функциональная_Особенность", "БТС_Функциональная_Особенность_Переопределить");
                                        //АТС_Покрытие_Окна_Спереди
                                        string window_Covering_In_Front = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Покрытие_Двери_Спереди", "-");
                                        //АТС_Покрытие_Окна_Сзади
                                        string window_Covering_In_Back = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Покрытие_Двери_Сзади", "-"); ;
                                        //АТС_Дополнительные_Сведенья
                                        string additional_Information = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Дополнительные_Сведенья", "-");
                                        //АТП_Обозначение
                                        Glass_Window glass_Window = new Glass_Window(
                                            product_Type,
                                            material_Of_Profile_Elements,
                                            doorstep,
                                            double_Glazed_Window_Formula,
                                            element_Doors,
                                            opening_Method,
                                            type_Of_Filling,
                                            щpening_Inside_To_Outside,
                                            height,
                                            wight,
                                            fire_Resistance,
                                            insulation,
                                            functional_Feature,
                                            window_Covering_In_Front,
                                            window_Covering_In_Back,
                                            additional_Information);
                                        Data_Mark_Door.list_Group.Add(glass_Window);
                                    }
                                if (element_Doors != null && element_Doors.LookupParameter("ЮТС_Dynamo_ID").AsValueString() == "Специализированные тип 1")
                                {
                                    //АТС_Конструктивное_Исполнение
                                    string heat_Transfer_Resistance_Class = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Конструктивное_Исполнение", "БТС_Конструктивное_Исполнение_Переопределить");
                                    //АТС_Порог
                                    string doorstep = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Порог", "БТС_Порог_Переопределить");
                                    //АТС_Открывание_Лево_Право
                                    string opening_Left_Right = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Открывание_Лево_Право", "БТС_Открывание_Лево_Право_Переопределить");
                                    //АТС_Открывание_Внутрь_Наружу
                                    string opening_Inside_To_Outside = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Открывание_Внутрь_Наружу", "БТС_Открывание_Внутрь_Наружу_Переопределить");
                                    //Высота
                                    string height = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТР_Примерная_Высота", "-"), CultureInfo.InvariantCulture))).ToString();
                                    //Ширина
                                    string wight = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТР_Примерная_Ширина", "-"), CultureInfo.InvariantCulture))).ToString();
                                    //АТС_Огнестойкость
                                    string fire_Resistance = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Огнестойкость", "БТС_Огнестойкость_Переопределить");
                                    //АТС_Утепленность
                                    string insulation = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Утепленность", "БТС_Утепленность_Переопределить");
                                    //АТС_Функциональная_Особенность
                                    string functional_Feature = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Функциональная_Особенность", "БТС_Функциональная_Особенность_Переопределить");
                                    //АТС_Расположение_Внутреннее_Наружное
                                    string location_Indoor_Outdoor = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Расположение_Внутреннее_Наружное", "-");
                                    //АТС_Покрытие_Окна_Спереди
                                    string window_Covering_In_Front = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Покрытие_Двери_Спереди", "-");
                                    //АТС_Покрытие_Окна_Сзади
                                    string window_Covering_In_Back = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Покрытие_Двери_Сзади", "-");
                                    //АТС_Дополнительные_Сведенья
                                    string additional_Information = parameter_Name.Parameter_Name_Of_Element(element_Doors, "АТС_Дополнительные_Сведенья", "-");
                                    Glass_Window glass_Window = new Glass_Window(
                                        heat_Transfer_Resistance_Class,
                                        doorstep,
                                        opening_Left_Right,
                                        opening_Inside_To_Outside,
                                        height,
                                        element_Doors,
                                        wight,
                                        fire_Resistance,
                                        insulation,
                                        functional_Feature,
                                        location_Indoor_Outdoor,
                                        window_Covering_In_Front,
                                        window_Covering_In_Back,
                                        additional_Information);
                                    Data_Mark_Door.list_Group.Add(glass_Window);
                                }
                            }


                        }
                    }
                    newT1.Commit();
                }
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
        }
    }
    public class Sort_On_Mark_Window
    {
        public Sort_On_Mark_Window()
        {
            try
            {
                using (TransactionGroup transactionGroup = new TransactionGroup(Revit_Document_Mark_Window.Document, "Маркировка дверей "))
                {
                    transactionGroup.Start();
                   
                    List<Glass_Window> list = Data_Mark_Door.list_Group.OrderByDescending(
                        x => x.element_Doors.get_Parameter(Data_Mark_Door.guid_COD).AsDouble() * 304.8).ThenBy(
                        x => double.Parse(x.height, CultureInfo.InvariantCulture)).ThenBy(
                        x => double.Parse(x.wight, CultureInfo.InvariantCulture)).ThenBy(
                        x => x.material_Of_Profile_Elements).ThenBy(
                        x => x.product_Type).ThenBy(
                        x => x.the_Material_Of_The_Frame_Elements).ThenBy(
                        x => x.purpose_Of_The_Product).ThenBy(
                        x => x.heat_Transfer_Resistance_Class).ThenBy(
                        x => x.opening_Left_Right).ThenBy(
                        x => x.location_Indoor_Outdoor).ThenBy(
                        x => x.door_Execution).ThenBy(
                        x => x.operational_Pperformance_Class).ThenBy(
                        x => x.strength_Сlass).ThenBy(
                        x => x.additional_Designation_Of_The_Execution).ThenBy(
                        x => x.number_Of_Canvases).ThenBy(
                        x => x.the_Presence_Of_Glazing).ThenBy(
                        x => x.the_Double_Glazed_Unit_Formula).ThenBy(
                        x => x.doorstep).ThenBy(
                        x => x.double_Glazed_Window_Formula).ThenBy(
                        x => x.opening_Method).ThenBy(
                        x => x.type_Of_Filling).ThenBy(
                        x => x.opening_Inside_To_Outside).ThenBy(
                        x => x.fire_Resistance).ThenBy(
                        x => x.insulation).ThenBy(
                        x => x.functional_Feature).ThenBy(
                        x => x.window_Covering_In_Front).ThenBy(
                        x => x.window_Covering_In_Back).ThenBy(
                        x => x.additional_Information).ThenBy(
                        x => x.element_Doors.Id.ToString()).ToList();

        list.Reverse();
                    foreach (Glass_Window glass_Window in list)
                    {
                        using (Transaction transaction1 = new Transaction(Revit_Document_Mark_Window.Document, "Транзакция 1"))
                        {
                            transaction1.Start();
                            Parameter parameter_ADSK_Mark = glass_Window.element_Doors.get_Parameter(Data_Mark_Door.guid_ADSK_Mark);
                            double parameter_ZH_COD = glass_Window.element_Doors.get_Parameter(Data_Mark_Door.guid_COD).AsDouble()*304.8;
                            string mark_Prefix = "Д-";
                            int number_Set = 0;
                            if (208.949 < parameter_ZH_COD && parameter_ZH_COD < 209)
                            {
                                Data_Mark_Door.number_Elements_Balcony++;
                                number_Set = Data_Mark_Door.number_Elements_Balcony;
                                mark_Prefix = "Б-";
                            }
                            if (206.999 < parameter_ZH_COD && parameter_ZH_COD < 208.950)
                            {
                                Data_Mark_Door.number_Elements++;
                                number_Set = Data_Mark_Door.number_Elements;
                            }
                            if (parameter_ADSK_Mark != null)
                            {
                                SSDK_Parameter.Set_Parameter(parameter_ADSK_Mark, mark_Prefix + number_Set.ToString());
                                SSDK_Parameter.Set_Type_Name (glass_Window.element_Doors, (mark_Prefix + number_Set.ToString() + " " + glass_Window.element_Doors.get_Parameter(Data_Mark_Door.guid_ADSK_NAME).AsValueString()));
                            }
                            if (parameter_ADSK_Mark == null&& Data_Mark_Door.iteration_Recaive_Value_In_Parameter == false)
                            {
                                Data_Mark_Door.iteration_Recaive_Value_In_Parameter = true;
                                Data_Mark_Door.iteration_Recaive_Value_In_Parameter_Watringn = Data_Mark_Door.iteration_Recaive_Value_In_Parameter_Watringn + "\n - Ошибка. Параметр 'ADSK_Марка' не найден!";
                            }
                            transaction1.Commit();
                        }
                    }
                    transactionGroup.Assimilate();
                }
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
        }
    }
    public class Parameter_Name
    {
        public string Parameter_Name_Of_Element(Element element, string name, string name_Remove)
        {
            try
            {
                string str = "";

                if (element.LookupParameter(name_Remove) != null)
                {
                    if (element.LookupParameter(name_Remove).AsValueString() != "Нет")
                    {
                        str = " " + element.LookupParameter(name_Remove).AsValueString();
                    }
                    else
                    {
                        if (element.LookupParameter(name) != null)
                        {
                            string[] previev_Name = element.LookupParameter(name).AsValueString().Split(new[] { "&" }, StringSplitOptions.None);
                            str = " " + previev_Name[previev_Name.Count() - 1];
                        }
                        else
                        {
                            if (Data_Mark_Door.iteration_Recaive_Value_In_Parameter == false)
                            {
                                Data_Mark_Door.iteration_Recaive_Value_In_Parameter = true;
                                Data_Mark_Door.iteration_Recaive_Value_In_Parameter_Watringn = Data_Mark_Door.iteration_Recaive_Value_In_Parameter_Watringn + "\n - Ошибка. Параметр' " + name + "' не найден. Обратитесь в BIM координатору";
                                return "";
                            }
                        }
                    }
                }
                else
                {
                    if (element.LookupParameter(name) != null)
                    {
                        string[] previev_Name = element.LookupParameter(name).AsValueString().Split(new[] { "&" }, StringSplitOptions.None);
                        str = " " + previev_Name[previev_Name.Count() - 1];
                    }
                    else
                    {
                        if (Data_Mark_Door.iteration_Recaive_Value_In_Parameter == false)
                        {
                            Data_Mark_Door.iteration_Recaive_Value_In_Parameter = true;
                            Data_Mark_Door.iteration_Recaive_Value_In_Parameter_Watringn = Data_Mark_Door.iteration_Recaive_Value_In_Parameter_Watringn + "\n - Ошибка. Параметр' " + name + " не найден. Обратитесь в BIM координатору";
                            return "";
                        }
                    }
                }
                if (str == " *" || str == " Нет")
                {
                    str = "";
                }
                if(str == null)
                {
                    Data_Mark_Door.iteration_Recaive_Value_In_Parameter = true;
                    Data_Mark_Door.iteration_Recaive_Value_In_Parameter_Watringn = Data_Mark_Door.iteration_Recaive_Value_In_Parameter_Watringn + "\n - Ошибка. При инициализации значения параметра'" + name + "' неожиданно возвращено значение null. Обратитесь в BIM координатору";
                    return "";
                }
                return str;
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
                return null;
            }
        }
    }
    public class IgnoreWarningPreprocessor : IFailuresPreprocessor
    {
        public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
        {
            // Получаем все предупреждения
            IList<FailureMessageAccessor> failureMessages = failuresAccessor.GetFailureMessages();

            foreach (FailureMessageAccessor failure in failureMessages)
            {
                // Удаляем предупреждение
                failuresAccessor.DeleteWarning(failure);
            }

            // Указываем продолжать выполнение
            return FailureProcessingResult.Continue;
        }
    }
}

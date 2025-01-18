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

namespace WPFApplication.Mark_Window
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
                    if (Data_Mark_Window.filtered_Group.Count > 0)
                    {
                        foreach (Element element_Window in Data_Mark_Window.filtered_Group)
                        {
                            Parameter_Name parameter_Name = new Parameter_Name();
                            if (element_Window != null && element_Window.LookupParameter("ЮТС_Dynamo_ID").AsValueString() == "ГОСТ_23166_О_Новое")
                            {
                                //Высота
                                string height = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТР_Примерная_Высота", "-");
                                //Ширина
                                string wight = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТР_Примерная_Ширина", "БТС_Тип_Изделия_Переопределить");
                                //АТС_Тип_Изделия
                                string product_Type = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Тип_Изделия", "БТС_Тип_Изделия_Переопределить");
                                //АТС_Материал_Профильных_Элементов
                                string material_Of_Profile_Elements = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Материал_Профильных_Элементов", "БТС_Материал_Профильных_Элементов_Переопределить");
                                //АТС_Тип_Конструкции
                                string type_Of_Construction = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Тип_Конструкции", "БТС_Тип_Конструкции_Переопределить");
                                //АТС_Формула_Стеклопакета
                                string double_Glazed_Window_Formula = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Формула_Стеклопакета", "БТС_Формула_Стеклопакета_Переопределить");
                                //АТС_Спобос_Открывания
                                string opening_Method = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Способ_Открывания", "БТС_Способ_Открывания_Переопределить");
                                //АТС_Покрытие_Окна_Спереди
                                string window_Covering_In_Front = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Покрытие_Окна_Спереди", "-");
                                //АТС_Покрытие_Окна_Сзади
                                string window_Covering_In_Back = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Покрытие_Окна_Сзади", "-"); ;
                                //АТС_Огнестойкость
                                string fire_Resistance = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Огнестойкость", "БТС_Огнестойкость_Переопределить");
                                //АТС_Утепленность
                                string insulation = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Утепленность", "БТС_Утепленность_Переопределить");
                                //АТС_Функциональная_Особенность
                                string functional_Feature = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Функциональная_Особенность", "БТС_Функциональная_Особенность_Переопределить");
                                //АТС_Расположение_Внутреннее_Наружное
                                string the_Location_Is_Internal = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Расположение_Внутреннее_Наружное", "-");
                                //АТС_Дополнительные_Сведенья
                                string additional_Information = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Дополнительные_Сведенья", "-");
                                //АТП_Обозначение
                                Glass_Window glass_Window = new Glass_Window(
                                    element_Window,
                                    height,
                                    wight,
                                    product_Type,
                                    material_Of_Profile_Elements,
                                    type_Of_Construction,
                                    double_Glazed_Window_Formula,
                                    opening_Method,
                                    window_Covering_In_Front,
                                    window_Covering_In_Back,
                                    fire_Resistance,
                                    insulation,
                                    functional_Feature,
                                    the_Location_Is_Internal,
                                    additional_Information);
                                Data_Mark_Window.list_Group.Add(glass_Window);
                            }
                            if (element_Window != null && element_Window.LookupParameter("ЮТС_Dynamo_ID").AsValueString() == "ГОСТ_30674_О_Новое")
                            {
                                //АТС_Тип_Изделия
                                string product_Type = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Вип_Изделия", "БТС_Вип_Изделия_Переопределить");
                                //Высота
                                string height = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТР_Примерная_Высота", "-");
                                //Ширина
                                string wight = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТР_Примерная_Ширина", "-");
                                //АТС_Класс_Сопротивления_Теплопередаче
                                string heat_Transfer_Resistance_Class = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Класс_Сопротивления_Теплопередаче", "БТС_Класс_Сопротивления_Теплопередаче_Переопределить");
                                //АТС_Формула_Стеклопакета
                                string the_Double_Glazed_Unit_Formula = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Формула_Стеклопакета", "БТС_Формула_Стеклопакета_Переопределить");
                                //АТС_Огнестойкость
                                string fire_Resistance = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Огнестойкость", "БТС_Огнестойкость_Переопределить");
                                //АТС_Покрытие_Окна_Спереди
                                string window_Covering_In_Front = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Покрытие_Окна_Спереди", "-");
                                //АТС_Покрытие_Окна_Сзади
                                string window_Covering_In_Back = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Покрытие_Окна_Сзади", "-"); ;
                                //АТС_Утепленность
                                string insulation = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Утепленность", "БТС_Утепленность_Переопределить");
                                //АТС_Функциональная_Особенность
                                string functional_Feature = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Функциональная_Особенность", "БТС_Функциональная_Особенность_Переопределить");
                                //АТС_Расположение_Внутреннее_Наружное
                                string location_Indoor_Outdoor = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Расположение_Внутреннее_Наружное", "-");
                                //АТС_Дополнительные_Сведенья
                                string additional_Information = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Дополнительные_Сведенья", "-");
                                Glass_Window glass_Window = new Glass_Window(
                                    height,
                                    element_Window,
                                    wight,
                                    product_Type,
                                    heat_Transfer_Resistance_Class,
                                    the_Double_Glazed_Unit_Formula,
                                    fire_Resistance,
                                    window_Covering_In_Front,
                                    window_Covering_In_Back,
                                    insulation,
                                    functional_Feature,
                                    location_Indoor_Outdoor,
                                    additional_Information);
                                Data_Mark_Window.list_Group.Add(glass_Window);
                            }

                            if (element_Window != null && element_Window.LookupParameter("ЮТС_Dynamo_ID").AsValueString() == "ГОСТ_30734_2020_О")
                            {
                                //Высота
                                string height = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТР_Примерная_Высота", "-");
                                //Ширина
                                string wight = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТР_Примерная_Ширина", "БТС_Тип_Изделия_Переопределить");
                                //АТС_Материал_Рамочных_Элементов
                                string the_Material_Of_The_Frame_Elements = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Материал_Рамочных_Элементов", "БТС_Материал_Рамочных_Элементов_Переопределить");
                                //АТС_Формула_Стеклопакета
                                string the_Double_Glazed_Unit_Formula = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Формула_Стеклопакета", "БТС_Формула_Стеклопакета_Переопределить");
                                //АТС_Покрытие_Окна_Спереди
                                string window_Covering_In_Front = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Покрытие_Окна_Спереди", "-");
                                //АТС_Покрытие_Окна_Сзади
                                string window_Covering_In_Back = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Покрытие_Окна_Сзади", "-"); ;
                                //АТС_Огнестойкость
                                string fire_Resistance = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Огнестойкость", "БТС_Огнестойкость_Переопределить");
                                //АТС_Утепленность
                                string insulation = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Утепленность", "БТС_Утепленность_Переопределить");
                                //АТС_Функциональная_Особенность
                                string functional_Feature = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Функциональная_Особенность", "БТС_Функциональная_Особенность_Переопределить");
                                //АТС_Расположение_Внутреннее_Наружное
                                string location_Indoor_Outdoor = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Расположение_Внутреннее_Наружное", "-");
                                //АТС_Дополнительные_Сведенья
                                string additional_Information = parameter_Name.Parameter_Name_Of_Element(element_Window, "АТС_Дополнительные_Сведенья", "-");
                                //АТП_Обозначение
                                Glass_Window glass_Window = new Glass_Window(
                                    height,
                                    wight,
                                    element_Window,
                                    the_Material_Of_The_Frame_Elements,
                                    the_Double_Glazed_Unit_Formula,
                                    window_Covering_In_Front,
                                    window_Covering_In_Back,
                                    fire_Resistance,
                                    insulation,
                                    functional_Feature,
                                    location_Indoor_Outdoor,
                                    additional_Information);
                                Data_Mark_Window.list_Group.Add(glass_Window);
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
                using (TransactionGroup transactionGroup = new TransactionGroup(Revit_Document_Mark_Window.Document, "Маркировка окон "))
                {
                    transactionGroup.Start();
                   
                    List<Glass_Window> list = Data_Mark_Window.list_Group.OrderBy(
                        x => double.Parse(x.height, CultureInfo.InvariantCulture)).ThenBy(
                        x => double.Parse(x.wight, CultureInfo.InvariantCulture)).ThenBy(
                        x => x.product_Type).ThenBy(
                        x => x.material_Of_Profile_Elements).ThenBy(
                        x => x.type_Of_Construction).ThenBy(
                        x => x.double_Glazed_Window_Formula).ThenBy(
                        x => x.opening_Method).ThenBy(
                        x => x.window_Covering_In_Front).ThenBy(
                        x => x.window_Covering_In_Back).ThenBy(
                        x => x.fire_Resistance).ThenBy(
                        x => x.insulation).ThenBy(
                        x => x.functional_Feature).ThenBy(
                        x => x.the_Location_Is_Internal).ThenBy(
                        x => x.additional_Information).ThenBy(
                        x => x.heat_Transfer_Resistance_Class).ThenBy(
                        x => x.the_Double_Glazed_Unit_Formula).ThenBy(
                        x => x.location_Indoor_Outdoor).ThenBy(
                        x => x.the_Material_Of_The_Frame_Elements).ThenBy(
                        x => x.element_Window.Id.ToString()).ToList();
                    list.Reverse();
                    foreach (Glass_Window glass_Window in list)
                    {
                        using (Transaction transaction1 = new Transaction(Revit_Document_Mark_Window.Document, "Транзакция 1"))
                        {
                            transaction1.Start();
                            if (Data_Mark_Window.error_Suppressio == true)
                            {
                                // Настройка для подавления предупреждений
                                FailureHandlingOptions failureOptions = transaction1.GetFailureHandlingOptions();
                                failureOptions.SetFailuresPreprocessor(new IgnoreWarningPreprocessor());
                                transaction1.SetFailureHandlingOptions(failureOptions);
                            }
                            Parameter parameter_ADSK_Mark = glass_Window.element_Window.get_Parameter(Data_Mark_Window.guid_ADSK_Mark);
                            string mark_Prefix = "";
                            int mark_Position = 0;
                            if (glass_Window.element_Window.LookupParameter("ЮТС_Фрамуга") != null)
                            {
                                if (glass_Window.element_Window.LookupParameter("ЮТС_Фрамуга").AsInteger() == 1)
                                {
                                    mark_Prefix = "Ф-";
                                    Data_Mark_Window.number_Elements_Transom++;
                                    mark_Position = Data_Mark_Window.number_Elements_Transom;
                                }
                                else
                                {
                                    mark_Prefix = "ОК-";
                                    Data_Mark_Window.number_Elements++;
                                    mark_Position = Data_Mark_Window.number_Elements;
                                }
                            }
                            else
                            {
                                mark_Prefix = "ОК-";
                                Data_Mark_Window.number_Elements++;
                                mark_Position = Data_Mark_Window.number_Elements;
                            }
                            if (parameter_ADSK_Mark != null)
                            {
                                if (parameter_ADSK_Mark.AsValueString().Trim() != (mark_Prefix + mark_Position.ToString()).Trim())
                                {
                                    parameter_ADSK_Mark.Set(mark_Prefix + mark_Position.ToString());
                                }
                                if (glass_Window.element_Window.Name.Trim() != (mark_Prefix + mark_Position.ToString() + " " + glass_Window.element_Window.get_Parameter(Data_Mark_Window.guid_ADSK_NAME).AsValueString()).Trim())
                                {
                                    glass_Window.element_Window.Name = (mark_Prefix + mark_Position.ToString() + " " + glass_Window.element_Window.get_Parameter(Data_Mark_Window.guid_ADSK_NAME).AsValueString());
                                }
                            }
                            if (parameter_ADSK_Mark == null&& Data_Mark_Window.iteration_Recaive_Value_In_Parameter == false)
                            {
                                Data_Mark_Window.iteration_Recaive_Value_In_Parameter = true;
                                Data_Mark_Window.iteration_Recaive_Value_In_Parameter_Watringn = Data_Mark_Window.iteration_Recaive_Value_In_Parameter_Watringn + "\n - Ошибка. Параметр 'ADSK_Марка' не найден!";
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
                            if (Data_Mark_Window.iteration_Recaive_Value_In_Parameter == false)
                            {
                                Data_Mark_Window.iteration_Recaive_Value_In_Parameter = true;
                                Data_Mark_Window.iteration_Recaive_Value_In_Parameter_Watringn = Data_Mark_Window.iteration_Recaive_Value_In_Parameter_Watringn + "\n - Ошибка. Параметр' " + name + "' не найден. Обратитесь в BIM координатору";
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
                        if (Data_Mark_Window.iteration_Recaive_Value_In_Parameter == false)
                        {
                            Data_Mark_Window.iteration_Recaive_Value_In_Parameter = true;
                            Data_Mark_Window.iteration_Recaive_Value_In_Parameter_Watringn = Data_Mark_Window.iteration_Recaive_Value_In_Parameter_Watringn + "\n - Ошибка. Параметр' " + name + " не найден. Обратитесь в BIM координатору";
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
                    Data_Mark_Window.iteration_Recaive_Value_In_Parameter = true;
                    Data_Mark_Window.iteration_Recaive_Value_In_Parameter_Watringn = Data_Mark_Window.iteration_Recaive_Value_In_Parameter_Watringn + "\n - Ошибка. При инициализации значения параметра'" + name + "' неожиданно возвращено значение null. Обратитесь в BIM координатору";
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
}

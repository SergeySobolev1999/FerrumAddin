﻿using Autodesk.Revit.DB;
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
using System.Windows.Controls;
using WPFApplication.Mark_On_Group_Stained_Glass_Windows;
using SSDK;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.DependencyInjection;
using System.Globalization;



namespace WPFApplication.Parameter_Window
{
    public class exParameter
    {
        public exParameter()
        {
            try
            {
                FilteredElementCollector window = new FilteredElementCollector(Revit_Document_Parameter_Window.Document);
                ICollection<Element> all_Elements = window.OfCategory(BuiltInCategory.OST_Windows).WhereElementIsNotElementType().ToElements();
                List<Element> filtered_Window = new List<Element>();
                foreach (Element element in all_Elements)
                {
                    Element element_Type = Revit_Document_Parameter_Window.Document.GetElement(element.GetTypeId());
                    if (Revit_Document_Parameter_Window.Document.GetElement(element.GetTypeId()) != null && element_Type.get_Parameter(Data_Parameter_Window.guid_COD) != null)
                    {

                        double parameter_Value = element_Type.get_Parameter(Data_Parameter_Window.guid_COD).AsDouble() * 304.8;
                        if (209 < parameter_Value && parameter_Value < 210.999 && element_Type.LookupParameter("ЮТС_Dynamo_ID") != null)
                        {
                            filtered_Window.Add(element);
                        }
                    }
                }
                if(filtered_Window.Count>0)
                {
                    using (Transaction transaction_Attribute = new Transaction(Revit_Document_Parameter_Window.Document, "Транзакция 1"))
                    {
                        transaction_Attribute.Start();
                        foreach (Element element in filtered_Window)
                        {
                            IList<string > strings_All = new List<string>();
                            string material_Boxes_Front = "";
                            if (element.get_Parameter(Data_Parameter_Window.guid_ADSK_POSITION) != null)
                            {
                                string material_Value_Parameter = SSDK_Parameter.Get_Parameter_Material_To_String(element.get_Parameter(Data_Parameter_Window.guid_Material_Boxes_Front));
                                if (material_Value_Parameter != "" ) 
                                {
                                   
                                    if (material_Value_Parameter.Contains("&"))
                                    {
                                        string[] material_Massiv = material_Value_Parameter.Split(new[] { "&" }, StringSplitOptions.None);
                                        material_Boxes_Front = material_Massiv[material_Massiv.Count() - 1];
                                    }
                                    List<string> wordsToRename = new List<string> { "Краска акриловая "
                                        , "Краска "
                                        , "Краска алкидная масляная "
                                        , "Краска алкидная масляная"
                                        , "Краска алкидная эмалевая "
                                        , "Краска алкидная эмалевая"
                                        , "Краска водно-дисперсионная "
                                        , "Краска водно-дисперсионная"
                                        , "Краска водоэмульсионная "
                                        , "Краска водоэмульсионная"
                                        , "Краска декстринированная "
                                        , "Краска декстринированная"
                                        , "Краска казеиновая "
                                        , "Краска казеиновая"
                                        , "Краска клеевая "
                                        , "Краска клеевая"
                                        , "Краска латексная "
                                        , "Краска латексная"
                                        , "Краска поливинилацетатная "
                                        , "Краска поливинилацетатная"
                                        , "Краска полиуретановая "
                                        , "Краска полиуретановая"
                                        , "Краска силикатная "
                                        , "Краска силикатная"
                                        , "Краска силиконовая "
                                        , "Краска силиконовая"
                                        , "Краска акриловая "
                                        , "Краска акриловая"
                                        , "Пленка ПВХ "
                                        , "Пленка ПВХ"};
                                    material_Boxes_Front = "Цвет коробки спереди: " + SSDK_Parameter.RenameWordsInString(material_Boxes_Front, wordsToRename, "");
                                }
                                if (material_Boxes_Front != "Цвет коробки спереди: ")
                                {
                                    strings_All.Add(material_Boxes_Front);
                                }
                                else 
                                {
                                    strings_All.Add ("");
                                }
                            }
                          
                            string finishing_String = "";
                            int iterarion = 0;
                            while (iterarion<strings_All.Count)
                            {
                                if (strings_All[iterarion].Length > 0&& iterarion!= strings_All.Count-2)
                                {
                                    finishing_String += strings_All[iterarion] + ", ";
                                }
                                else
                                {
                                    finishing_String += strings_All[iterarion];
                                }
                                iterarion++;
                            }
                            element.get_Parameter(Data_Parameter_Window.guid_ADSK_POSITION).Set(finishing_String);
                             

                        }
                        transaction_Attribute.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
        }
    }
    public class Collecting_Windows
    {
        public Collecting_Windows()
        {
            try
            {
                using (TransactionGroup transactionGroup = new TransactionGroup(Revit_Document_Parameter_Window.Document, "Выгрузка данных формата "))
                {
                    transactionGroup.Start();
                    if (Data_Parameter_Window.filtered_Group.Count > 0)
                    {
                        foreach (Element element_Group in Data_Parameter_Window.filtered_Group)
                        {
                            //if (Revit_Document_Parameter_Window.Document.GetElement(element_Group.Id) != null)
                            //{
                            if (element_Group != null && element_Group.LookupParameter("ЮТС_Dynamo_ID").AsValueString() == "ГОСТ_23166_О_Новое"|| element_Group.LookupParameter("ЮТС_Dynamo_ID").AsValueString() == "ГОСТ_23166_К_Новое")
                            {
                                Parameter_Name parameter_Name = new Parameter_Name();

                                //АТС_Тип_Изделия
                                string product_Type = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Тип_Изделия", "БТС_Тип_Изделия_Переопределить");
                                    
                                //Высота
                                string height = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Group, "АТР_Примерная_Высота", "-"), CultureInfo.InvariantCulture))).ToString();
                                    
                                    //Ширина
                                string wight = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Group, "АТР_Примерная_Ширина", "-"), CultureInfo.InvariantCulture))).ToString();
                                if (element_Group.LookupParameter("ЮТС_Dynamo_ID").AsValueString() == "ГОСТ_23166_К_Новое")
                                {
                                    height = "d" + height;
                                    wight = "";
                                }
                                if (element_Group.LookupParameter("ЮТС_Dynamo_ID").AsValueString() == "ГОСТ_23166_О_Новое")
                                {
                                    height =  height + "х";
                                }
                                //АТС_Материал_Профильных_Элементов
                                string material_Of_Profile_Elements = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Материал_Профильных_Элементов", "БТС_Материал_Профильных_Элементов_Переопределить");
                                //АТС_Тип_Конструкции
                                string type_Of_Construction = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Тип_Конструкции", "БТС_Тип_Конструкции_Переопределить");
                                //АТС_Формула_Стеклопакета
                                string double_Glazed_Window_Formula = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Формула_Стеклопакета", "БТС_Формула_Стеклопакета_Переопределить");
                                //АТС_Спобос_Открывания
                                string opening_Method = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Способ_Открывания", "БТС_Способ_Открывания_Переопределить");
                                //АТС_Покрытие_Окна_Спереди
                                string window_Covering_In_Front = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Покрытие_Окна_Спереди", "-");
                                if (window_Covering_In_Front.Length > 0)
                                {
                                    window_Covering_In_Front = " Покрытие спереди -" + window_Covering_In_Front;
                                }
                                //АТС_Покрытие_Окна_Сзади
                                string window_Covering_In_Back = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Покрытие_Окна_Сзади", "-"); 
                                if (window_Covering_In_Back.Length > 0)
                                {
                                    window_Covering_In_Back = " Покрытие сзади -" + window_Covering_In_Back;
                                }
                                //АТС_Огнестойкость
                                string fire_Resistance = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Огнестойкость", "БТС_Огнестойкость_Переопределить");
                                //АТС_Утепленность
                                string insulation = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Утепленность", "БТС_Утепленность_Переопределить");
                                //АТС_Функциональная_Особенность
                                string functional_Feature = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Функциональная_Особенность", "БТС_Функциональная_Особенность_Переопределить");
                                //АТС_Расположение_Внутреннее_Наружное
                                string the_Location_Is_Internal = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Расположение_Внутреннее_Наружное", "-");
                                //АТС_Дополнительные_Сведенья
                                string additional_Information = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Дополнительные_Сведенья", "-");
                                //АТП_Обозначение
                                string[] stoc_Designation_Perview = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТП_Обозначение", "ВТС_Обозначение_Переопределить").Split(new[] { ":" }, StringSplitOptions.None);
                                string stoc_Designation = " " + stoc_Designation_Perview[stoc_Designation_Perview.Count() - 1];
                                string name_Presset = "";
                                if (Data_Parameter_Window.sketch_bool = true)
                                {
                                    name_Presset = "";
                                }
                                string result_Name = name_Presset+ product_Type + " " + height  + wight + material_Of_Profile_Elements + type_Of_Construction + "(" + double_Glazed_Window_Formula + ")" + opening_Method + window_Covering_In_Front +
                                    window_Covering_In_Back + fire_Resistance + insulation + functional_Feature + the_Location_Is_Internal + additional_Information;
                            using (Transaction transaction1 = new Transaction(Revit_Document_Parameter_Window.Document, "Транзакция 1"))
                            {
                                transaction1.Start();
                                    if (Data_Parameter_Window.error_Suppressio == true)
                                    {
                                        // Настройка для подавления предупреждений
                                        FailureHandlingOptions failureOptions = transaction1.GetFailureHandlingOptions();
                                        failureOptions.SetFailuresPreprocessor(new IgnoreWarningPreprocessor());
                                        transaction1.SetFailureHandlingOptions(failureOptions);
                                    }
                                    SSDK_Parameter.Set_Parameter(element_Group.get_Parameter(Data_Parameter_Window.guid_ADSK_Mark), "");
                                    SSDK_Parameter.Set_Parameter(element_Group.get_Parameter(Data_Parameter_Window.guid_ADSK_NAME), result_Name);
                                    SSDK_Parameter.Set_Parameter(element_Group.get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_COMMENTS), fire_Resistance);
                                    SSDK_Parameter.Set_Parameter(element_Group.get_Parameter(Data_Parameter_Window.guid_ADSK_Desination), stoc_Designation);
                                    SSDK_Parameter.Set_Type_Name(element_Group, result_Name + " (" + Data_Parameter_Window.number_Elements.ToString() + ")");
                                    Data_Parameter_Window.number_Elements++;
                                    transaction1.Commit();
                            }
                            }
                            if (element_Group != null && element_Group.LookupParameter("ЮТС_Dynamo_ID").AsValueString() == "ГОСТ_30674_О_Новое")
                            {
                                Parameter_Name parameter_Name = new Parameter_Name();

                                //АТС_Тип_Изделия
                                string product_Type = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Вид_Изделия", "БТС_Вип_Изделия_Переопределить");
                                //Высота
                                string height = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Group, "АТР_Примерная_Высота", "-"), CultureInfo.InvariantCulture))).ToString();
                                //Ширина
                                string wight = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Group, "АТР_Примерная_Ширина", "-"), CultureInfo.InvariantCulture))).ToString();
                                //АТС_Класс_Сопротивления_Теплопередаче
                                string heat_Transfer_Resistance_Class = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Класс_Сопротивления_Теплопередаче", "БТС_Класс_Сопротивления_Теплопередаче_Переопределить");
                                //АТС_Формула_Стеклопакета
                                string the_Double_Glazed_Unit_Formula = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Формула_Стеклопакета", "БТС_Формула_Стеклопакета_Переопределить");
                                //АТС_Огнестойкость
                                string fire_Resistance = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Огнестойкость", "БТС_Огнестойкость_Переопределить");
                                //АТС_Покрытие_Окна_Спереди
                                string window_Covering_In_Front = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Покрытие_Окна_Спереди", "-");
                                if (window_Covering_In_Front.Length > 0)
                                {
                                    window_Covering_In_Front = " Покрытие спереди -" + window_Covering_In_Front;
                                }
                                //АТС_Покрытие_Окна_Сзади
                                string window_Covering_In_Back = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Покрытие_Окна_Сзади", "-");
                                if (window_Covering_In_Back.Length > 0)
                                {
                                    window_Covering_In_Back = " Покрытие сзади -" + window_Covering_In_Back;
                                }
                                //АТС_Утепленность
                                string insulation = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Утепленность", "БТС_Утепленность_Переопределить");
                                //АТС_Функциональная_Особенность
                                string functional_Feature = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Функциональная_Особенность", "БТС_Функциональная_Особенность_Переопределить");
                                //АТС_Расположение_Внутреннее_Наружное
                                string location_Indoor_Outdoor = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Расположение_Внутреннее_Наружное", "-");
                                //АТС_Дополнительные_Сведенья
                                string additional_Information = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Дополнительные_Сведенья", "-");
                                //АТП_Обозначение
                                string[] stoc_Designation_Perview = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТП_Обозначение", "ВТС_Обозначение_Переопределить").Split(new[] { ":" }, StringSplitOptions.None);
                                string stoc_Designation = " " + stoc_Designation_Perview[stoc_Designation_Perview.Count() - 1];
                                string name_Presset = "";
                                if (Data_Parameter_Window.sketch_bool = true)
                                {
                                    name_Presset = "";
                                }
                                string result_Name = name_Presset+ product_Type + " " + height + "х" + wight + heat_Transfer_Resistance_Class + " (" + the_Double_Glazed_Unit_Formula + ")" + fire_Resistance + window_Covering_In_Front +
                                    window_Covering_In_Back + insulation + functional_Feature + location_Indoor_Outdoor + additional_Information;
                                using (Transaction transaction1 = new Transaction(Revit_Document_Parameter_Window.Document, "Транзакция 1"))
                                {
                                    transaction1.Start();
                                    if (Data_Parameter_Window.error_Suppressio == true)
                                    {
                                        // Настройка для подавления предупреждений
                                        FailureHandlingOptions failureOptions = transaction1.GetFailureHandlingOptions();
                                        failureOptions.SetFailuresPreprocessor(new IgnoreWarningPreprocessor());
                                        transaction1.SetFailureHandlingOptions(failureOptions);
                                    }
                                    SSDK_Parameter.Set_Parameter(element_Group.get_Parameter(Data_Parameter_Window.guid_ADSK_Mark), "");
                                    SSDK_Parameter.Set_Parameter(element_Group.get_Parameter(Data_Parameter_Window.guid_ADSK_NAME), result_Name);
                                    SSDK_Parameter.Set_Parameter(element_Group.get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_COMMENTS), fire_Resistance);
                                    SSDK_Parameter.Set_Parameter(element_Group.get_Parameter(Data_Parameter_Window.guid_ADSK_Desination), stoc_Designation);
                                    SSDK_Parameter.Set_Type_Name(element_Group, result_Name + " (" + Data_Parameter_Window.number_Elements.ToString() + ")");
                                    Data_Parameter_Window.number_Elements++;
                                    transaction1.Commit();
                                }
                            }
                            if (element_Group != null && element_Group.LookupParameter("ЮТС_Dynamo_ID").AsValueString() == "ГОСТ_30734_2020_О")
                            {
                                Parameter_Name parameter_Name = new Parameter_Name();

                                //АТС_Материал_Рамочных_Элементов
                                string the_Material_Of_The_Frame_Elements = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Материал_Рамочных_Элементов", "БТС_Материал_Рамочных_Элементов_Переопределить");
                                //Высота
                                string height = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Group, "АТР_Примерная_Высота", "-"), CultureInfo.InvariantCulture))).ToString();
                                //Ширина
                                string wight = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Group, "АТР_Примерная_Ширина", "-"), CultureInfo.InvariantCulture))).ToString();
                                //АТС_Класс_Сопротивления_Теплопередаче
                                string heat_Transfer_Resistance_Class = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Вариант_Открывания_Створки", "БТС_Вариант_Открывания_Створки_Переопределить");
                                //АТС_Формула_Стеклопакета
                                string the_Double_Glazed_Unit_Formula = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Формула_Стеклопакета", "БТС_Формула_Стеклопакета_Переопределить");
                                //АТС_Покрытие_Окна_Спереди
                                string window_Covering_In_Front = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Покрытие_Окна_Спереди", "-");
                                if (window_Covering_In_Front.Length > 0)
                                {
                                    window_Covering_In_Front = " Покрытие спереди -" + window_Covering_In_Front;
                                }
                                //АТС_Покрытие_Окна_Сзади
                                string window_Covering_In_Back = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Покрытие_Окна_Сзади", "-");
                                if (window_Covering_In_Back.Length > 0)
                                {
                                    window_Covering_In_Back = " Покрытие сзади -" + window_Covering_In_Back;
                                }
                                //АТС_Огнестойкость
                                string fire_Resistance = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Огнестойкость", "БТС_Огнестойкость_Переопределить");
                                //АТС_Утепленность
                                string insulation = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Утепленность", "БТС_Утепленность_Переопределить");
                                //АТС_Функциональная_Особенность
                                string functional_Feature = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Функциональная_Особенность", "БТС_Функциональная_Особенность_Переопределить");
                                //АТС_Расположение_Внутреннее_Наружное
                                string location_Indoor_Outdoor = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Расположение_Внутреннее_Наружное", "-");
                                //АТС_Дополнительные_Сведенья
                                string additional_Information = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Дополнительные_Сведенья", "-");
                                //АТП_Обозначение
                                string[] stoc_Designation_Perview = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТП_Обозначение", "ВТС_Обозначение_Переопределить").Split(new[] { ":" }, StringSplitOptions.None);
                                string stoc_Designation = " " + stoc_Designation_Perview[stoc_Designation_Perview.Count() - 1];
                                string name_Presset = "";
                                if (Data_Parameter_Window.sketch_bool = true)
                                {
                                    name_Presset = "";
                                }
                                string result_Name = name_Presset+ the_Material_Of_The_Frame_Elements + " " + height + "х" + wight + heat_Transfer_Resistance_Class + " (" + the_Double_Glazed_Unit_Formula + ")"  + window_Covering_In_Front +
                                    window_Covering_In_Back + fire_Resistance + insulation + functional_Feature + location_Indoor_Outdoor + additional_Information;
                                using (Transaction transaction1 = new Transaction(Revit_Document_Parameter_Window.Document, "Транзакция 1"))
                                {
                                    transaction1.Start();
                                    if (Data_Parameter_Window.error_Suppressio == true)
                                    {
                                        // Настройка для подавления предупреждений
                                        FailureHandlingOptions failureOptions = transaction1.GetFailureHandlingOptions();
                                        failureOptions.SetFailuresPreprocessor(new IgnoreWarningPreprocessor());
                                        transaction1.SetFailureHandlingOptions(failureOptions);
                                    }

                                    SSDK_Parameter.Set_Parameter(element_Group.get_Parameter(Data_Parameter_Window.guid_ADSK_Mark), "");
                                    SSDK_Parameter.Set_Parameter(element_Group.get_Parameter(Data_Parameter_Window.guid_ADSK_NAME), result_Name);
                                    SSDK_Parameter.Set_Parameter(element_Group.get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_COMMENTS), fire_Resistance);
                                    SSDK_Parameter.Set_Parameter(element_Group.get_Parameter(Data_Parameter_Window.guid_ADSK_Desination), stoc_Designation);
                                    SSDK_Parameter.Set_Type_Name(element_Group, result_Name + " (" + Data_Parameter_Window.number_Elements.ToString() + ")");
                                    Data_Parameter_Window.number_Elements++;
                                    transaction1.Commit();
                                }
                            }
                            //}
                            Data_Parameter_Window.number_Elements++;
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
                            if (Data_Parameter_Window.iteration_Recaive_Value_In_Parameter == false)
                            {
                                Data_Parameter_Window.iteration_Recaive_Value_In_Parameter = true;
                                Data_Parameter_Window.iteration_Recaive_Value_In_Parameter_Watringn = Data_Parameter_Window.iteration_Recaive_Value_In_Parameter_Watringn + "\n - Ошибка. Параметр' " + name + "' не найден. Обратитесь в BIM координатору";
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
                        if (Data_Parameter_Window.iteration_Recaive_Value_In_Parameter == false)
                        {
                            Data_Parameter_Window.iteration_Recaive_Value_In_Parameter = true;
                            Data_Parameter_Window.iteration_Recaive_Value_In_Parameter_Watringn = Data_Parameter_Window.iteration_Recaive_Value_In_Parameter_Watringn + "\n - Ошибка. Параметр' " + name + " не найден. Обратитесь в BIM координатору";
                            return "";
                        }
                    }
                }
                if (str == " *" || str == " Нет" || str == "" || str == " ")
                {
                    str = "";
                }
                if (str == null)
                {
                    Data_Parameter_Window.iteration_Recaive_Value_In_Parameter = true;
                    Data_Parameter_Window.iteration_Recaive_Value_In_Parameter_Watringn = Data_Parameter_Window.iteration_Recaive_Value_In_Parameter_Watringn + "\n - Ошибка. При инициализации значения параметра'" + name + "' неожиданно возвращено значение null. Обратитесь в BIM координатору";
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

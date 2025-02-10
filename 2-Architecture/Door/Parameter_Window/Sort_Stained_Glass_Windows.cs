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
using System.Windows.Controls;
using WPFApplication.Mark_On_Group_Stained_Glass_Windows;
using SSDK;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.DependencyInjection;
using System.CodeDom.Compiler;
using Autodesk.Revit.DB.Visual;
using static Autodesk.Revit.DB.SpecTypeId;
using System.Globalization;
using WPFApplication.Parameter_On_Group_Stained_Glass_Windows;


namespace WPFApplication.Parameter_Door
{
    public class Collecting_Windows
    {
        public Collecting_Windows()
        {
            try
            {
                using (TransactionGroup transactionGroup = new TransactionGroup(Revit_Document_Parameter_Window.Document, "Выгрузка данных формата "))
                {
                    transactionGroup.Start();
                  
                    if (Data_Parameter_Door.filtered_Group.Count > 0)
                    {

                        foreach (Element element_Familyinstace in Data_Parameter_Door.filtered_Group)
                        {
                            if (Revit_Document_Parameter_Window.Document.GetElement(element_Familyinstace.GetTypeId()) != null)
                            {
                                Element element_Group_Ex = Revit_Document_Parameter_Window.Document.GetElement(element_Familyinstace.Id);
                                Element element_Group = Revit_Document_Parameter_Window.Document.GetElement(element_Familyinstace.GetTypeId());
                                
                                if (element_Group.LookupParameter("ЮТС_Dynamo_ID").AsValueString() == "ГОСТ_23747_Д_Новое")
                                {
                                    Parameter_Name parameter_Name = new Parameter_Name();

                                    //АТС_Тип_Изделия
                                    string product_Type = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Назначение_Изделия", "БТС_Назначение_Изделия_Переопределить");
                                    //АТС_Конструктивное_Исполнение
                                    string material_Of_Profile_Elements = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Конструктивное_Исполнение", "БТС_Конструктивное_Исполнение_Переопределить");
                                    //АТС_Порог
                                    string type_Of_Construction = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Порог", "БТС_Порог_Переопределить");
                                    //АТС_Открывание_Лево_Право
                                    string double_Glazed_Window_Formula = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Открывание_Лево_Право", "БТС_Открывание_Лево_Право_Переопределить");
                                    //АТС_Спобос_Открывания
                                    string opening_Method = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Способ_Открывания", "БТС_Способ_Открывания_Переопределить");
                                    //АТС_Вид_Заполнения
                                    string type_Of_Filling = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Вид_Заполнения", "БТС_Вид_Заполнения_Переопределить");
                                    //АТС_Открывание_Внутрь_Наружу
                                    string щpening_Inside_To_Outside = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Открывание_Внутрь_Наружу", "БТС_Открывание_Внутрь_Наружу_Переопределить");
                                    //Высота
                                    string height = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Group, "АТР_Примерная_Высота", "-"), CultureInfo.InvariantCulture))).ToString();
                                    //Ширина
                                    string wight = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Group, "АТР_Примерная_Ширина", "-"), CultureInfo.InvariantCulture))).ToString();
                                    //АТС_Огнестойкость
                                    string fire_Resistance = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Огнестойкость", "БТС_Огнестойкость_Переопределить");
                                    //АТС_Утепленность
                                    string insulation = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Утепленность", "БТС_Утепленность_Переопределить");
                                    //АТС_Функциональная_Особенность
                                    string functional_Feature = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Функциональная_Особенность", "БТС_Функциональная_Особенность_Переопределить");
                                    //АТС_Покрытие_Окна_Спереди
                                    string window_Covering_In_Front = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Покрытие_Двери_Спереди", "-");
                                    if(window_Covering_In_Front.Length > 0)
                                    {
                                        window_Covering_In_Front = " Покрытие спереди -" +window_Covering_In_Front;
                                    }
                                    //АТС_Покрытие_Окна_Сзади
                                    string window_Covering_In_Back = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Покрытие_Двери_Сзади", "-"); ;
                                    if (window_Covering_In_Back.Length > 0)
                                    {
                                        window_Covering_In_Back = " Покрытие сзади -" + window_Covering_In_Back;
                                    }
                                    //АТС_Дополнительные_Сведенья
                                    string additional_Information = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Дополнительные_Сведенья", "-");
                                    //АТП_Обозначение
                                    string[] stoc_Designation_Perview = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТП_Обозначение", "ВТС_Обозначение_Переопределить").Split(new[] { ":" }, StringSplitOptions.None);
                                    string stoc_Designation = " " + stoc_Designation_Perview[stoc_Designation_Perview.Count() - 1];
                                    string result_Name = product_Type + material_Of_Profile_Elements + type_Of_Construction + double_Glazed_Window_Formula + opening_Method + type_Of_Filling + щpening_Inside_To_Outside + " " + height + "х" +
                                        wight + fire_Resistance + insulation + functional_Feature + window_Covering_In_Front+ window_Covering_In_Back + additional_Information;
                                    using (Transaction transaction1 = new Transaction(Revit_Document_Parameter_Window.Document, "Транзакция 1"))
                                    {
                                        transaction1.Start();
                                        if (Data_Parameter_Door.error_Suppressio == true)
                                        {
                                            // Настройка для подавления предупреждений
                                            FailureHandlingOptions failureOptions = transaction1.GetFailureHandlingOptions();
                                            failureOptions.SetFailuresPreprocessor(new IgnoreWarningPreprocessor());
                                            transaction1.SetFailureHandlingOptions(failureOptions);
                                        }
                                        if (element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_Mark).AsValueString().Trim() != "")
                                        {
                                            element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_Mark).Set("");
                                        }
                                        if (element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_NAME).AsValueString().Trim() != result_Name.Trim())
                                        {
                                            element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_NAME).Set(result_Name);
                                        }
                                        //if (!element_Group.get_Parameter(BuiltInParameter.DOOR_FIRE_RATING).AsValueString().Trim().Contains(fire_Resistance.Trim()))
                                        //{
                                            element_Group.get_Parameter(BuiltInParameter.DOOR_FIRE_RATING).Set(fire_Resistance);
                                        //}
                                        if (stoc_Designation.Trim() != element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_Desination).AsValueString().Trim())
                                        {
                                            if (stoc_Designation != null)
                                            {
                                                element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_Desination).Set(stoc_Designation);
                                            }
                                        }
                                        if (result_Name != null)
                                        {
                                            element_Group.Name = result_Name + " (" + Data_Parameter_Door.number_Elements.ToString() + ")";
                                        }
                                        Data_Parameter_Door.number_Elements++;
                                        transaction1.Commit();
                                    }
                                }

                                if (element_Group != null && element_Group.LookupParameter("ЮТС_Dynamo_ID").AsValueString() == "ГОСТ_57327_Д_Новое")
                                {
                                    Parameter_Name parameter_Name = new Parameter_Name();

                                    //АТС_Назначение_Изделия
                                    string purpose_Of_The_Product = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Назначение_Изделия", "БТС_Назначение_Изделия_Переопределить");
                                    //АТС_Конструктивное_Исполнение
                                    string heat_Transfer_Resistance_Class = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Конструктивное_Исполнение", "БТС_Конструктивное_Исполнение_Переопределить");
                                    //АТС_Открывание_Лево_Право
                                    string opening_Left_Right = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Открывание_Лево_Право", "БТС_Открывание_Лево_Право_Переопределить");
                                    //АТС_Открывание_Внутрь_Наружу
                                    string щpening_Inside_To_Outside = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Открывание_Внутрь_Наружу", "БТС_Открывание_Внутрь_Наружу_Переопределить");
                                    //Высота
                                    string height = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Group, "АТР_Примерная_Высота", "-"), CultureInfo.InvariantCulture))).ToString();
                                    //Ширина
                                    string wight = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Group, "АТР_Примерная_Ширина", "-"), CultureInfo.InvariantCulture))).ToString();
                                    //АТС_Огнестойкость
                                    string fire_Resistance = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Огнестойкость", "БТС_Огнестойкость_Переопределить");
                                    //АТС_Утепленность
                                    string insulation = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Утепленность", "БТС_Утепленность_Переопределить");
                                    //АТС_Функциональная_Особенность
                                    string functional_Feature = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Функциональная_Особенность", "БТС_Функциональная_Особенность_Переопределить");
                                    //АТС_Расположение_Внутреннее_Наружное
                                    string location_Indoor_Outdoor = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Расположение_Внутреннее_Наружное", "-");
                                    //АТС_Покрытие_Окна_Спереди
                                    string window_Covering_In_Front = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Покрытие_Двери_Спереди", "-");
                                    if (window_Covering_In_Front.Length > 0)
                                    {
                                        window_Covering_In_Front = " Покрытие спереди -" + window_Covering_In_Front;
                                    }
                                    //АТС_Покрытие_Окна_Сзади
                                    string window_Covering_In_Back = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Покрытие_Двери_Сзади", "-"); ;
                                    if (window_Covering_In_Back.Length > 0)
                                    {
                                        window_Covering_In_Back = " Покрытие сзади -" + window_Covering_In_Back;
                                    }
                                    //АТС_Дополнительные_Сведенья
                                    string additional_Information = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Дополнительные_Сведенья", "-");
                                    //АТП_Обозначение
                                    string[] stoc_Designation_Perview = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТП_Обозначение", "ВТС_Обозначение_Переопределить").Split(new[] { ":" }, StringSplitOptions.None);
                                    string stoc_Designation = " " + stoc_Designation_Perview[stoc_Designation_Perview.Count() - 1];
                                    string result_Name = purpose_Of_The_Product + heat_Transfer_Resistance_Class + opening_Left_Right + щpening_Inside_To_Outside + " " + height + "х" + wight +
                                        fire_Resistance + insulation + functional_Feature + location_Indoor_Outdoor + window_Covering_In_Front + window_Covering_In_Back + additional_Information;
                                    using (Transaction transaction1 = new Transaction(Revit_Document_Parameter_Window.Document, "Транзакция 1"))
                                    {
                                        transaction1.Start();
                                        if (Data_Parameter_Door.error_Suppressio == true)
                                        {
                                            // Настройка для подавления предупреждений
                                            FailureHandlingOptions failureOptions = transaction1.GetFailureHandlingOptions();
                                            failureOptions.SetFailuresPreprocessor(new IgnoreWarningPreprocessor());
                                            transaction1.SetFailureHandlingOptions(failureOptions);
                                        }
                                        if (element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_Mark).AsValueString().Trim() != "")
                                        {
                                            element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_Mark).Set("");
                                        }
                                        if (element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_NAME).AsValueString().Trim() != result_Name.Trim())
                                        {
                                            element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_NAME).Set(result_Name);
                                        }
                                        //if (!element_Group.get_Parameter(BuiltInParameter.DOOR_FIRE_RATING).AsValueString().Trim().Contains(fire_Resistance.Trim()))
                                        //{
                                            element_Group.get_Parameter(BuiltInParameter.DOOR_FIRE_RATING).Set(fire_Resistance);
                                        //}
                                        if (stoc_Designation.Trim() != element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_Desination).AsValueString().Trim())
                                        {
                                            if (stoc_Designation != null)
                                            {
                                                element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_Desination).Set(stoc_Designation);
                                            }
                                        }
                                        if (result_Name != null)
                                        {
                                            element_Group.Name = result_Name + " (" + Data_Parameter_Door.number_Elements.ToString() + ")";
                                        }
                                        Data_Parameter_Door.number_Elements++;
                                        transaction1.Commit();
                                    }
                                }
                                if (element_Group != null && element_Group.LookupParameter("ЮТС_Dynamo_ID").AsValueString() == "ГОСТ_31173_Д_Новое")
                                {
                                    Parameter_Name parameter_Name = new Parameter_Name();

                                    //АТС_Назначение_Изделия
                                    string the_Material_Of_The_Frame_Elements = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Назначение_Изделия", "БТС_Назначение_Изделия_Переопределить");
                                    //АТС_Конструктивное_Исполнение
                                    string heat_Transfer_Resistance_Class = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Конструктивное_Исполнение", "БТС_Конструктивное_Исполнение_Переопределить");
                                    //АТС_Порог
                                    string the_Double_Glazed_Unit_Formula = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Порог", "БТС_Порог_Переопределить");
                                    //АТС_Открывание_Лево_Право
                                    string opening_Left_Right = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Открывание_Лево_Право", "БТС_Открывание_Лево_Право_Переопределить");
                                    //АТС_Открывание_Внутрь_Наружу
                                    string щpening_Inside_To_Outside = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Открывание_Внутрь_Наружу", "БТС_Открывание_Внутрь_Наружу_Переопределить");
                                    //АТС_Исполнение_Двери
                                    string door_Execution = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Исполнение_Двери", "БТС_Исполнение_Двери_Переопределить");
                                    //АТС_Класс_По_Эксплуатационным_Характеристикам
                                    string operational_Pperformance_Class = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Класс_По_Эксплуатационным_Характеристикам", "БТС_Класс_По_Эксплуатационным_Характеристикам");
                                    //АТС_Класс_Прочности
                                    string strength_Сlass = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Класс_Прочности", "БТС_Класс_Прочности_Переопределить");
                                    //АТС_Дополнительное_Обозначение_Исполнения
                                    string additional_Designation_Of_The_Execution = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Дополнительное_Обозначение_Исполнения", "БТС_Дополнительное_Обозначение_Исполнения_Переопределить");
                                    //Высота
                                    string height = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Group, "АТР_Примерная_Высота", "-"), CultureInfo.InvariantCulture))).ToString();
                                    //Ширина
                                    string wight = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Group, "АТР_Примерная_Ширина", "-"), CultureInfo.InvariantCulture))).ToString();
                                    //АТС_Огнестойкость
                                    string fire_Resistance = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Огнестойкость", "БТС_Огнестойкость_Переопределить");
                                    //АТС_Утепленность
                                    string insulation = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Утепленность", "БТС_Утепленность_Переопределить");
                                    //АТС_Функциональная_Особенность
                                    string functional_Feature = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Функциональная_Особенность", "БТС_Функциональная_Особенность_Переопределить");
                                    //АТС_Расположение_Внутреннее_Наружное
                                    string location_Indoor_Outdoor = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Расположение_Внутреннее_Наружное", "-");
                                    //АТС_Покрытие_Окна_Спереди
                                    string window_Covering_In_Front = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Покрытие_Двери_Спереди", "-");
                                    if (window_Covering_In_Front.Length > 0)
                                    {
                                        window_Covering_In_Front = " Покрытие спереди -" + window_Covering_In_Front;
                                    }
                                    //АТС_Покрытие_Окна_Сзади
                                    string window_Covering_In_Back = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Покрытие_Двери_Сзади", "-");
                                    if (window_Covering_In_Back.Length > 0)
                                    {
                                        window_Covering_In_Back = " Покрытие сзади -" + window_Covering_In_Back;
                                    }
                                    //АТС_Дополнительные_Сведенья
                                    string additional_Information = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Дополнительные_Сведенья", "-");
                                    //АТП_Обозначение
                                    string[] stoc_Designation_Perview = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТП_Обозначение", "ВТС_Обозначение_Переопределить").Split(new[] { ":" }, StringSplitOptions.None);
                                    string stoc_Designation = " " + stoc_Designation_Perview[stoc_Designation_Perview.Count() - 1];
                                    string result_Name = the_Material_Of_The_Frame_Elements + heat_Transfer_Resistance_Class + the_Double_Glazed_Unit_Formula + opening_Left_Right + щpening_Inside_To_Outside + door_Execution +
                                        operational_Pperformance_Class + strength_Сlass + additional_Designation_Of_The_Execution + " " + height + "х" + wight + fire_Resistance + insulation + functional_Feature + location_Indoor_Outdoor + window_Covering_In_Front + window_Covering_In_Back + additional_Information;
                                    using (Transaction transaction1 = new Transaction(Revit_Document_Parameter_Window.Document, "Транзакция 1"))
                                    {
                                        transaction1.Start();
                                        if (Data_Parameter_Door.error_Suppressio == true)
                                        {
                                            // Настройка для подавления предупреждений
                                            FailureHandlingOptions failureOptions = transaction1.GetFailureHandlingOptions();
                                            failureOptions.SetFailuresPreprocessor(new IgnoreWarningPreprocessor());
                                            transaction1.SetFailureHandlingOptions(failureOptions);
                                        }
                                        if (element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_Mark).AsValueString().Trim() != "")
                                        {
                                            element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_Mark).Set("");
                                        }
                                        if (!element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_NAME).AsValueString().Trim().Contains(result_Name.Trim()))
                                        {
                                            element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_NAME).Set(result_Name);
                                        }
                                        //if (!element_Group.get_Parameter(BuiltInParameter.DOOR_FIRE_RATING).AsValueString().Trim().Contains(fire_Resistance.Trim()))
                                        //{
                                            element_Group.get_Parameter(BuiltInParameter.DOOR_FIRE_RATING).Set(fire_Resistance);
                                        //}
                                        if (stoc_Designation.Trim() != element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_Desination).AsValueString().Trim())
                                        {
                                            if (stoc_Designation != null)
                                            {
                                                element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_Desination).Set(stoc_Designation);
                                            }
                                        }
                                        if (result_Name != null)
                                        {
                                            element_Group.Name = result_Name + " (" + Data_Parameter_Door.number_Elements.ToString() + ")";
                                        }
                                        Data_Parameter_Door.number_Elements++;
                                        transaction1.Commit();
                                    }
                                }
                                if (element_Group != null && element_Group.LookupParameter("ЮТС_Dynamo_ID").AsValueString() == "ГОСТ_475_Д_Новое")
                                {
                                    Parameter_Name parameter_Name = new Parameter_Name();

                                    //АТС_Назначение_Изделия
                                    string the_Material_Of_The_Frame_Elements = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Назначение_Изделия", "БТС_Назначение_Изделия_Переопределить");
                                    //АТС_Направление_И_Способ_Открывания
                                    string heat_Transfer_Resistance_Class = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Направление_И_Способ_Открывания", "БТС_Направление_И_Способ_Открывания_Переопределить");
                                    //АТС_Число_Полотен
                                    string number_Of_Canvases = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Число_Полотен", "БТС_Число_Полотен_Переопределить");
                                    //БТС_Наличие_Остекления_Переопределить
                                    string the_Presence_Of_Glazing = parameter_Name.Parameter_Name_Of_Element(element_Group, "БТС_Наличие_Остекления_Переопределить", "БТС_Наличие_Остекления_Переопределить");
                                    //АТС_Порог
                                    string the_Double_Glazed_Unit_Formula = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Порог", "БТС_Порог_Переопределить");
                                    //АТС_Открывание_Внутрь_Наружу
                                    string щpening_Inside_To_Outside = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Открывание_Внутрь_Наружу", "БТС_Открывание_Внутрь_Наружу_Переопределить");
                                    //Высота
                                    string height = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Group, "АТР_Примерная_Высота", "-"), CultureInfo.InvariantCulture))).ToString();
                                    //Ширина
                                    string wight = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Group, "АТР_Примерная_Ширина", "-"), CultureInfo.InvariantCulture))).ToString();
                                    //АТС_Огнестойкость
                                    string fire_Resistance = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Огнестойкость", "БТС_Огнестойкость_Переопределить");
                                    //АТС_Утепленность
                                    string insulation = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Утепленность", "БТС_Утепленность_Переопределить");
                                    //АТС_Функциональная_Особенность
                                    string functional_Feature = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Функциональная_Особенность", "БТС_Функциональная_Особенность_Переопределить");
                                    //АТС_Покрытие_Окна_Спереди
                                    string window_Covering_In_Front = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Покрытие_Двери_Спереди", "-");
                                    if (window_Covering_In_Front.Length > 0)
                                    {
                                        window_Covering_In_Front = " Покрытие спереди -" + window_Covering_In_Front;
                                    }
                                    //АТС_Покрытие_Окна_Сзади
                                    string window_Covering_In_Back = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Покрытие_Двери_Сзади", "-");
                                    if (window_Covering_In_Back.Length > 0)
                                    {
                                        window_Covering_In_Back = " Покрытие сзади -" + window_Covering_In_Back;
                                    }
                                    //АТС_Дополнительные_Сведенья
                                    string additional_Information = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Дополнительные_Сведенья", "-");
                                    //АТП_Обозначение
                                    string[] stoc_Designation_Perview = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТП_Обозначение", "ВТС_Обозначение_Переопределить").Split(new[] { ":" }, StringSplitOptions.None);
                                    string stoc_Designation = " " + stoc_Designation_Perview[stoc_Designation_Perview.Count() - 1];
                                    string result_Name = the_Material_Of_The_Frame_Elements + heat_Transfer_Resistance_Class + number_Of_Canvases + the_Presence_Of_Glazing + the_Double_Glazed_Unit_Formula + щpening_Inside_To_Outside + " " + height + "х" + wight + fire_Resistance + insulation + functional_Feature + window_Covering_In_Front + window_Covering_In_Back + additional_Information;
                                    using (Transaction transaction1 = new Transaction(Revit_Document_Parameter_Window.Document, "Транзакция 1"))
                                    {
                                        transaction1.Start();
                                        if (Data_Parameter_Door.error_Suppressio == true)
                                        {
                                            // Настройка для подавления предупреждений
                                            FailureHandlingOptions failureOptions = transaction1.GetFailureHandlingOptions();
                                            failureOptions.SetFailuresPreprocessor(new IgnoreWarningPreprocessor());
                                            transaction1.SetFailureHandlingOptions(failureOptions);
                                        }
                                        if (element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_Mark).AsValueString().Trim() != "")
                                        {
                                            element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_Mark).Set("");
                                        }
                                        if (!element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_NAME).AsValueString().Trim().Contains(result_Name.Trim()))
                                        {
                                            element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_NAME).Set(result_Name);
                                        }
                                        //if (!element_Group.get_Parameter(BuiltInParameter.DOOR_FIRE_RATING).AsValueString().Trim().Contains(fire_Resistance.Trim()))
                                        //{
                                            element_Group.get_Parameter(BuiltInParameter.DOOR_FIRE_RATING).Set(fire_Resistance);
                                        //}
                                        if (stoc_Designation.Trim() != element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_Desination).AsValueString().Trim())
                                        {
                                            if (stoc_Designation != null)
                                            {
                                                element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_Desination).Set(stoc_Designation);
                                            }
                                        }
                                        if (result_Name != null)
                                        {
                                            element_Group.Name = result_Name + " (" + Data_Parameter_Door.number_Elements.ToString() + ")";
                                        }
                                        Data_Parameter_Door.number_Elements++;
                                        transaction1.Commit();
                                    }
                                }
                                if (element_Group != null && (element_Group.LookupParameter("ЮТС_Dynamo_ID").AsValueString() == "ГОСТ_30674_ДБ_Новое" || element_Group.LookupParameter("ЮТС_Dynamo_ID").AsValueString() == "ГОСТ_30674_Д_Новое"))
                                {
                                    Parameter_Name parameter_Name = new Parameter_Name();

                                    //АТС_Вид_Изделия
                                    string product_Type = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Вид_Изделия", "БТС_Вид_Изделия_Переопределить");
                                    //АТС_Класс_Сопротивления_Теплопередаче
                                    string heat_Transfer_Resistance_Class = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Класс_Сопротивления_Теплопередаче", "БТС_Класс_Сопротивления_Теплопередаче_Переопределить");
                                    //Высота
                                    string height = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Group, "АТР_Примерная_Высота", "-"), CultureInfo.InvariantCulture))).ToString();
                                    //Ширина
                                    string wight = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Group, "АТР_Примерная_Ширина", "-"), CultureInfo.InvariantCulture))).ToString();
                                    //АТС_Конструктивное_Исполнение
                                    string number_Of_Canvases = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Конструктивное_Исполнение", "БТС_Конструктивное_Исполнение_Переопределить");
                                    //АТС_Открывание_Внутрь_Наружу
                                    string щpening_Inside_To_Outside = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Открывание_Внутрь_Наружу", "БТС_Открывание_Внутрь_Наружу_Переопределить");
                                    //АТС_Открывание_Лево_Право
                                    string opening_Left_Right = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Открывание_Лево_Право", "БТС_Открывание_Лево_Право_Переопределить");
                                    //АТС_Формула_Стеклопакета
                                    string the_Double_Glazed_Unit_Formula = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Формула_Стеклопакета", "БТС_Формула_Стеклопакета_Переопределить");
                                    //АТС_Огнестойкость
                                    string fire_Resistance = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Огнестойкость", "БТС_Огнестойкость_Переопределить");
                                    //АТС_Утепленность
                                    string insulation = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Утепленность", "БТС_Утепленность_Переопределить");
                                    //АТС_Функциональная_Особенность
                                    string functional_Feature = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Функциональная_Особенность", "БТС_Функциональная_Особенность_Переопределить");
                                    //АТС_Покрытие_Окна_Спереди
                                    string window_Covering_In_Front = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Покрытие_Двери_Спереди", "-");
                                    if (window_Covering_In_Front.Length > 0)
                                    {
                                        window_Covering_In_Front = " Покрытие спереди -" + window_Covering_In_Front;
                                    }
                                    //АТС_Покрытие_Окна_Сзади
                                    string window_Covering_In_Back = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Покрытие_Двери_Сзади", "-");
                                    if (window_Covering_In_Back.Length > 0)
                                    {
                                        window_Covering_In_Back = " Покрытие сзади -" + window_Covering_In_Back;
                                    }
                                    //АТС_Дополнительные_Сведенья
                                    string additional_Information = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Дополнительные_Сведенья", "-");
                                    //АТП_Обозначение
                                    string[] stoc_Designation_Perview = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТП_Обозначение", "ВТС_Обозначение_Переопределить").Split(new[] { ":" }, StringSplitOptions.None);
                                    string stoc_Designation = " " + stoc_Designation_Perview[stoc_Designation_Perview.Count() - 1];
                                    string result_Name = product_Type + heat_Transfer_Resistance_Class + " " + height + "х" + wight + number_Of_Canvases + щpening_Inside_To_Outside +
                                        opening_Left_Right + the_Double_Glazed_Unit_Formula + fire_Resistance + insulation + functional_Feature + window_Covering_In_Front + window_Covering_In_Back + additional_Information;
                                    using (Transaction transaction1 = new Transaction(Revit_Document_Parameter_Window.Document, "Транзакция 1"))
                                    {
                                        transaction1.Start();
                                        if (Data_Parameter_Door.error_Suppressio == true)
                                        {
                                            // Настройка для подавления предупреждений
                                            FailureHandlingOptions failureOptions = transaction1.GetFailureHandlingOptions();
                                            failureOptions.SetFailuresPreprocessor(new IgnoreWarningPreprocessor());
                                            transaction1.SetFailureHandlingOptions(failureOptions);
                                        }
                                        if (element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_Mark).AsValueString().Trim() != "")
                                        {
                                            element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_Mark).Set("");
                                        }
                                        if (!element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_NAME).AsValueString().Trim().Contains(result_Name.Trim()))
                                        {
                                            element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_NAME).Set(result_Name);
                                        }
                                        //if (!element_Group.get_Parameter(BuiltInParameter.DOOR_FIRE_RATING).AsValueString().Trim().Contains(fire_Resistance.Trim()))
                                        //{
                                        //    element_Group.get_Parameter(BuiltInParameter.DOOR_FIRE_RATING).Set(fire_Resistance);
                                        //}
                                        if (stoc_Designation.Trim() != element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_Desination).AsValueString().Trim())
                                        {
                                            if (stoc_Designation != null)
                                            {
                                                element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_Desination).Set(stoc_Designation);
                                            }
                                        }
                                        if (result_Name != null)
                                        {
                                            element_Group.Name = result_Name + " (" + Data_Parameter_Door.number_Elements.ToString() + ")";
                                        }
                                        Data_Parameter_Door.number_Elements++;
                                        transaction1.Commit();
                                    }

                                }
                                if (element_Group != null && element_Group.LookupParameter("ЮТС_Dynamo_ID").AsValueString() == "ГОСТ_23747_Д_Витражная_Новое")
                                {
                                    Parameter_Name parameter_Name = new Parameter_Name();

                                    //АТС_Тип_Изделия
                                    string product_Type = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Назначение_Изделия", "БТС_Назначение_Изделия_Переопределить");
                                    //АТС_Конструктивное_Исполнение
                                    string material_Of_Profile_Elements = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Конструктивное_Исполнение", "БТС_Конструктивное_Исполнение_Переопределить");
                                    //АТС_Порог
                                    string type_Of_Construction = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Порог", "БТС_Порог_Переопределить");
                                    //АТС_Открывание_Лево_Право
                                    string double_Glazed_Window_Formula = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Открывание_Лево_Право", "БТС_Открывание_Лево_Право_Переопределить");
                                    //АТС_Спобос_Открывания
                                    string opening_Method = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Способ_Открывания", "БТС_Способ_Открывания_Переопределить");
                                    //АТС_Вид_Заполнения
                                    string type_Of_Filling = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Вид_Заполнения", "БТС_Вид_Заполнения_Переопределить");
                                    //АТС_Открывание_Внутрь_Наружу
                                    string щpening_Inside_To_Outside = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Открывание_Внутрь_Наружу", "БТС_Открывание_Внутрь_Наружу_Переопределить");
                                    //Высота
                                    string height = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Group_Ex, "АЭР_Примерная_Высота", "-"), CultureInfo.InvariantCulture))).ToString();
                                    //Ширина
                                    string wight = ((double)Math.Round(double.Parse(parameter_Name.Parameter_Name_Of_Element(element_Group_Ex, "АЭР_Примерная_Ширина", "-"), CultureInfo.InvariantCulture))).ToString();
                                    //АТС_Огнестойкость
                                    string fire_Resistance = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Огнестойкость", "БТС_Огнестойкость_Переопределить");
                                    //АТС_Утепленность
                                    string insulation = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Утепленность", "БТС_Утепленность_Переопределить");
                                    //АТС_Функциональная_Особенность
                                    string functional_Feature = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Функциональная_Особенность", "БТС_Функциональная_Особенность_Переопределить");
                                    //АТС_Покрытие_Окна_Спереди
                                    string window_Covering_In_Front = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Покрытие_Двери_Спереди", "-");
                                    if (window_Covering_In_Front.Length > 0)
                                    {
                                        window_Covering_In_Front = "Покрытие спереди - " + window_Covering_In_Front;
                                    }
                                    //АТС_Покрытие_Окна_Сзади
                                    string window_Covering_In_Back = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Покрытие_Двери_Сзади", "-"); 
                                    if (window_Covering_In_Back.Length > 0)
                                    {
                                        window_Covering_In_Back = "Покрытие сзади - " + window_Covering_In_Back;
                                    }
                                    //АТС_Дополнительные_Сведенья
                                    string additional_Information = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Дополнительные_Сведенья", "-");
                                    //АТП_Обозначение
                                    string[] stoc_Designation_Perview = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТП_Обозначение", "ВТС_Обозначение_Переопределить").Split(new[] { ":" }, StringSplitOptions.None);
                                    string stoc_Designation = " " + stoc_Designation_Perview[stoc_Designation_Perview.Count() - 1];
                                    string result_Name = product_Type + material_Of_Profile_Elements + type_Of_Construction + double_Glazed_Window_Formula + opening_Method + type_Of_Filling + щpening_Inside_To_Outside + " " + height + "х" +
                                        wight + fire_Resistance + insulation + functional_Feature + window_Covering_In_Front + additional_Information;
                                    using (Transaction transaction1 = new Transaction(Revit_Document_Parameter_Window.Document, "Транзакция 1"))
                                    {
                                        transaction1.Start();
                                        if (Data_Parameter_Door.error_Suppressio == true)
                                        {
                                            // Настройка для подавления предупреждений
                                            FailureHandlingOptions failureOptions = transaction1.GetFailureHandlingOptions();
                                            failureOptions.SetFailuresPreprocessor(new IgnoreWarningPreprocessor());
                                            transaction1.SetFailureHandlingOptions(failureOptions);
                                        }
                                        if (element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_Mark).AsValueString().Trim() != "")
                                        {
                                            element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_Mark).Set("");
                                        }
                                        if (element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_NAME).AsValueString().Trim() !=result_Name.Trim())
                                        {
                                            element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_NAME).Set(result_Name);
                                        }
                                        //if (!element_Group.get_Parameter(BuiltInParameter.DOOR_FIRE_RATING).AsValueString().Trim().Contains(fire_Resistance.Trim()))
                                        //{
                                            element_Group.get_Parameter(BuiltInParameter.DOOR_FIRE_RATING).Set(fire_Resistance);
                                        //}
                                        if (stoc_Designation.Trim() != element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_Desination).AsValueString().Trim())
                                        {
                                            if (stoc_Designation != null)
                                            {
                                                element_Group.get_Parameter(Data_Parameter_Door.guid_ADSK_Desination).Set(stoc_Designation);
                                            }
                                        }
                                        if (result_Name != null)
                                        {
                                            element_Group.Name = result_Name + " (" + Data_Parameter_Door.number_Elements.ToString() + ")";
                                        }
                                        Data_Parameter_Door.number_Elements++;
                                        transaction1.Commit();
                                    }
                                }
                            }
                            
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
                            if (Data_Parameter_Door.iteration_Recaive_Value_In_Parameter == false)
                            {
                                Data_Parameter_Door.iteration_Recaive_Value_In_Parameter = true;
                                Data_Parameter_Door.iteration_Recaive_Value_In_Parameter_Watringn = Data_Parameter_Door.iteration_Recaive_Value_In_Parameter_Watringn + "\n - Ошибка. Параметр' " + name + "' не найден. Обратитесь в BIM координатору";
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
                        if (Data_Parameter_Door.iteration_Recaive_Value_In_Parameter == false)
                        {
                            Data_Parameter_Door.iteration_Recaive_Value_In_Parameter = true;
                            Data_Parameter_Door.iteration_Recaive_Value_In_Parameter_Watringn = Data_Parameter_Door.iteration_Recaive_Value_In_Parameter_Watringn + "\n - Ошибка. Параметр' " + name + " не найден. Обратитесь в BIM координатору";
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
                    Data_Parameter_Door.iteration_Recaive_Value_In_Parameter = true;
                    Data_Parameter_Door.iteration_Recaive_Value_In_Parameter_Watringn = Data_Parameter_Door.iteration_Recaive_Value_In_Parameter_Watringn + "\n - Ошибка. При инициализации значения параметра'" + name + "' неожиданно возвращено значение null. Обратитесь в BIM координатору";
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

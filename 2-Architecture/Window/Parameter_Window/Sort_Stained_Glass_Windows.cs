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

namespace WPFApplication.Parameter_Window
{
    public class Collecting_Group_Stained_Glass_Windows
    {
        public Collecting_Group_Stained_Glass_Windows()
        {
            try
            {
                using (Transaction newT1 = new Transaction(Revit_Document_Parameter_Window.Document, "Выгрузка данных формата "))
                {
                    newT1.Start();
                    if (Data_Parameter_Window.filtered_Group.Count > 0)
                    {  
                        foreach (Element element_Group in Data_Parameter_Window.filtered_Group)
                        {
                            if (Revit_Document_Parameter_Window.Document.GetElement(element_Group.Id) != null)
                            {
                                if (element_Group != null && element_Group.LookupParameter("ЮТС_Dynamo_ID").AsValueString() == "ГОСТ_23166_О_Новое")
                                {
                                    Parameter_Name parameter_Name = new Parameter_Name();
                                    //АТС_Тип_Изделия
                                    string product_Type = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Тип_Изделия", "БТС_Тип_Изделия_Переопределить");
                                    //Высота
                                    string height = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТР_Примерная_Высота", "-");
                                    //Ширина
                                    string wight = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТР_Примерная_Ширина", "БТС_Тип_Изделия_Переопределить");
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
                                    //АТС_Покрытие_Окна_Сзади
                                    string window_Covering_In_Back = parameter_Name.Parameter_Name_Of_Element(element_Group, "АТС_Покрытие_Окна_Сзади", "-"); ;
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
                                    string result_Name = product_Type + height + wight + material_Of_Profile_Elements + type_Of_Construction + "("+double_Glazed_Window_Formula+")" + opening_Method + window_Covering_In_Front +
                                        window_Covering_In_Back + fire_Resistance + insulation + functional_Feature + the_Location_Is_Internal + additional_Information;
                                    element_Group.get_Parameter(Data_Parameter_Window.guid_ADSK_Mark).Set("");
                                    element_Group.get_Parameter(Data_Parameter_Window.guid_ADSK_NAME).Set(result_Name);
                                    element_Group.get_Parameter(Data_Parameter_Window.guid_ADSK_Desination).Set(stoc_Designation);
                                    if (result_Name!=null)
                                    {
                                        element_Group.Name = result_Name;
                                    }
                                }
                            }
                            Data_Parameter_Window.number_Elements++;
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
    public class Parameter_Name
    {
        public string Parameter_Name_Of_Element(Element element,string name,string name_Remove)
        {
            try
            {
                string str = "";
                if(element.LookupParameter(name_Remove) != null)
                {
                    if (element.LookupParameter(name_Remove).AsValueString() != "Нет")
                    {
                        str = " "+element.LookupParameter(name_Remove).AsValueString();
                    }
                    else
                    {
                        string[] previev_Name = element.LookupParameter(name).AsValueString().Split(new[] { "&" }, StringSplitOptions.None);
                        str = " " + previev_Name[previev_Name.Count() - 1];
                    }
                }
                else
                {
                    string[] previev_Name = element.LookupParameter(name).AsValueString().Split(new[] { "&" }, StringSplitOptions.None);
                    str = " " + previev_Name[previev_Name.Count() - 1];
                }
               if(str ==" *"|| str == " Нет")
               {
                    str = "";
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

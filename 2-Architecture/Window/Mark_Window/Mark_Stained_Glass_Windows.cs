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

namespace WPFApplication.Mark_Window
{
    public class Collecting_Group_Stained_Glass_Windows
    {
        public Collecting_Group_Stained_Glass_Windows()
        {
            try
            {
                using (Transaction newT1 = new Transaction(Revit_Document_Mark_On_Group_Stained_Glass_Windows.Document, "Выгрузка данных формата "))
                {
                    newT1.Start();
                    if (Data_Mark_On_Group_Stained_Glass_Windows.filtered_Group.Count > 0)
                    {
                        foreach (Group element_Group in Data_Mark_On_Group_Stained_Glass_Windows.filtered_Group)
                        {
                            string description_Value = "";
                            string model_Value = "";
                            string model_Designation = "";
                            string height_Value = "0";
                            string wight_Value = "0";
                            string full_Name = "";
                            double cod = 0;
                            Guid guid_designation = new Guid("9c98831b-9450-412d-b072-7d69b39f4029");
                            Guid guid_COD = new Guid("631cd69e-065f-4ec2-8894-4359325312c3");
                            Guid guid_ADSK_NAME = new Guid("e6e0f5cd-3e26-485b-9342-23882b20eb43");
                            ICollection<ElementId> collection_ElementId_All_Grop_In_Model = element_Group.GetMemberIds();
                            List<Element> collection_Element_All_Grop_In_Model = new List<Element>();
                            foreach (ElementId elementId in collection_ElementId_All_Grop_In_Model)
                            {
                                collection_Element_All_Grop_In_Model.Add(Revit_Document_Mark_On_Group_Stained_Glass_Windows.Document.GetElement(elementId));
                            }
                            foreach (Element element_Id_In_Goup in collection_Element_All_Grop_In_Model)
                            {
                                if (element_Id_In_Goup.Category != null)
                                {
                                    if (element_Id_In_Goup.Category.Name.ToString() == "Стены")
                                    {
                                        Element element_Type = Revit_Document_Mark_On_Group_Stained_Glass_Windows.Document.GetElement(element_Id_In_Goup.GetTypeId());
                                        double wight_Position = 0;
                                        if (element_Type.get_Parameter(BuiltInParameter.ALL_MODEL_DESCRIPTION).AsValueString() != null)
                                        {
                                            description_Value = element_Type.get_Parameter(BuiltInParameter.ALL_MODEL_DESCRIPTION).AsValueString();
                                            description_Value = " " + description_Value.Replace("Теплый контур", "(утепл.)");
                                        }
                                        if (element_Type.get_Parameter(BuiltInParameter.ALL_MODEL_MODEL).AsValueString() != null)
                                        {
                                            model_Value = " " + element_Type.get_Parameter(BuiltInParameter.ALL_MODEL_MODEL).AsValueString();

                                        }
                                        if (double.Parse(element_Id_In_Goup.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsValueString()) > 0)
                                        {
                                            wight_Position = double.Parse(element_Id_In_Goup.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsValueString());
                                            wight_Value = (double.Parse(wight_Value) + wight_Position).ToString();
                                        }
                                        if (double.Parse(element_Id_In_Goup.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM).AsValueString()) > 0)
                                        {
                                            height_Value = " " + (element_Id_In_Goup.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM).AsValueString().ToString());
                                        }
                                        model_Designation = " " + element_Type.get_Parameter(guid_designation).AsValueString();
                                        cod = element_Type.get_Parameter(guid_COD).AsDouble();
                                        full_Name = model_Value + model_Designation + height_Value + "х" + wight_Value + description_Value;
                                    }
                                }
                            }
                            Element element_Type_Cod = Revit_Document_Mark_On_Group_Stained_Glass_Windows.Document.GetElement(element_Group.GetTypeId());

                            Parameter parameter_Value = element_Type_Cod.get_Parameter(guid_COD);
                            parameter_Value.Set(cod);

                            Parameter parameter_Value_Designation = element_Type_Cod.get_Parameter(guid_designation);
                            parameter_Value_Designation.Set(model_Designation);

                            Parameter parameter_Value_Name = element_Type_Cod.get_Parameter(guid_ADSK_NAME);
                            parameter_Value_Name.Set(full_Name);

                            Glass_Window glass_Window = new Glass_Window(description_Value, model_Value, model_Designation, height_Value, wight_Value, element_Group, full_Name);
                            Data_Mark_On_Group_Stained_Glass_Windows.list_Group.Add(glass_Window);
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
    public class Sort_On_Parameter
    {
        public Sort_On_Parameter()
        {
            try
            {
                using (Transaction newT1 = new Transaction(Revit_Document_Mark_On_Group_Stained_Glass_Windows.Document, "Выгрузка данных формата "))
                {
                    newT1.Start();
                    List<Glass_Window> list = Data_Mark_On_Group_Stained_Glass_Windows.list_Group.OrderBy(x => x.model_Value).ThenBy(x => double.Parse(x.height_Value)).ThenBy(x => double.Parse(x.wight_Value)).ToList();
                    list.Reverse();
                    int identical = 1;
                    int position_Mark = 1;
                    double last_Cod = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        string postscript = "";
                        Element element_Type = Revit_Document_Mark_On_Group_Stained_Glass_Windows.Document.GetElement(list[i].element.GetTypeId());
                        double parameter = Math.Round(element_Type.get_Parameter(Data_Mark_On_Group_Stained_Glass_Windows.guid_Group).AsDouble()*304.8,3);
                        if (parameter !=last_Cod&&last_Cod!=0)
                        {
                            identical = 1;
                            position_Mark = 1;
                        }
                        if (parameter == 212.001)
                        {
                            postscript = "ВВ-";
                        }
                        if (parameter == 211.001)
                        {
                            postscript = "ВН-";
                        }
                        if (parameter == 211.002)
                        {
                            postscript = "ОЛ-";
                        }
                        Group element_Group = (Group)Revit_Document_Mark_On_Group_Stained_Glass_Windows.Document.GetElement(list[i].element.Id);

                        GroupType groupType = element_Group.GroupType;
                        groupType.Name = postscript  + position_Mark.ToString() + " " + list[i].full_Name.ToString();
                        FilteredElementCollector window = new FilteredElementCollector(Revit_Document_Mark_On_Group_Stained_Glass_Windows.Document);
                        ICollection<Element> all_Elements = window.OfCategory(BuiltInCategory.OST_IOSModelGroups).WhereElementIsNotElementType().ToElements();
                        foreach (Element element in all_Elements)
                        {
                            if (Revit_Document_Mark_On_Group_Stained_Glass_Windows.Document.GetElement(element.GetTypeId()).Name.ToString() == groupType.Name.ToString())
                            {
                                Parameter parameter_ADSK_Mark = element.get_Parameter(Data_Mark_On_Group_Stained_Glass_Windows.guid_ADSK_Mark);
                                parameter_ADSK_Mark.Set(postscript + position_Mark.ToString());
                            }
                        }
                        position_Mark++;
                        last_Cod = parameter;
                        Data_Mark_On_Group_Stained_Glass_Windows.number_Elements++;
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
}

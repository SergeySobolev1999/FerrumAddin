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
using SSDK;
using WPFApplication.Parameter_Window;

namespace WPFApplication.Parameter_On_Group_Stained_Glass_Windows
{
  
    public class Collecting_Group_Stained_Glass_Windows
    {
        public Collecting_Group_Stained_Glass_Windows()
        {
            try
            {
                using (Transaction newT1 = new Transaction(Revit_Document_Parameter_On_Group_Stained_Glass_Windows.Document, "Выгрузка данных формата "))
                {
                    newT1.Start();
                    if (Data_Parameter_On_Group_Stained_Glass_Windows.error_Suppressio == true)
                    {
                        // Настройка для подавления предупреждений
                        FailureHandlingOptions failureOptions = newT1.GetFailureHandlingOptions();
                        failureOptions.SetFailuresPreprocessor(new IgnoreWarningPreprocessor());
                        newT1.SetFailureHandlingOptions(failureOptions);
                    }
                    if (Data_Parameter_On_Group_Stained_Glass_Windows.filtered_Group.Count > 0)
                    {  
                        foreach (Group element_Group in Data_Parameter_On_Group_Stained_Glass_Windows.filtered_Group)
                        {
                            string description_Value = "";
                            string model_Value = "";
                            string model_Designation = "";
                            string height_Value = "0";
                            string wight_Value = "0";
                            string full_Name = "";
                            string full_Name_Type = "";
                            double cod = 0;
                            Guid guid_designation = new Guid("9c98831b-9450-412d-b072-7d69b39f4029");
                            Guid guid_COD = new Guid("631cd69e-065f-4ec2-8894-4359325312c3");
                            Guid guid_ADSK_NAME = new Guid("e6e0f5cd-3e26-485b-9342-23882b20eb43");
                            ICollection <ElementId> collection_ElementId_All_Grop_In_Model = element_Group.GetMemberIds();
                            List<Element> collection_Element_All_Grop_In_Model = new List<Element>();
                            foreach (ElementId elementId in collection_ElementId_All_Grop_In_Model)
                            {
                                collection_Element_All_Grop_In_Model.Add(Revit_Document_Parameter_On_Group_Stained_Glass_Windows.Document.GetElement(elementId));
                            }
                            foreach (Element element_Id_In_Goup in collection_Element_All_Grop_In_Model)
                            {
                                if (element_Id_In_Goup.Category != null)
                                {
                                    if (element_Id_In_Goup.Category.Name.ToString() == "Стены")
                                    {
                                        Element element_Type = Revit_Document_Parameter_On_Group_Stained_Glass_Windows.Document.GetElement(element_Id_In_Goup.GetTypeId());
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
                                        full_Name = model_Value +  height_Value + "х" + wight_Value + description_Value;
                                        full_Name_Type = model_Value + model_Designation + height_Value + "х" + wight_Value + description_Value;
                                    }
                                }
                            }
                            Element element_Type_Cod = Revit_Document_Parameter_On_Group_Stained_Glass_Windows.Document.GetElement(element_Group.GetTypeId());
                    
                            Parameter parameter_Value = element_Type_Cod.get_Parameter(guid_COD);
                            parameter_Value.Set(cod);

                            SSDK_Parameter.Set_Parameter(element_Type_Cod.get_Parameter(guid_designation), model_Designation);
                            FilteredElementCollector window = new FilteredElementCollector(Revit_Document_Parameter_On_Group_Stained_Glass_Windows.Document);
                            ICollection<Element> all_Elements = window.OfCategory(BuiltInCategory.OST_IOSModelGroups).WhereElementIsNotElementType().ToElements();
                            
                            foreach (Element element in all_Elements)
                            {
                                if (Revit_Document_Parameter_On_Group_Stained_Glass_Windows.Document.GetElement(element.GetTypeId()).Name.ToString() == element_Type_Cod.Name.ToString())
                                {
                                    SSDK_Parameter.Set_Parameter(element.get_Parameter(Data_Parameter_On_Group_Stained_Glass_Windows.guid_ADSK_Mark), "");
                                }
                            }
                            SSDK_Parameter.Set_Parameter(element_Type_Cod.get_Parameter(guid_ADSK_NAME), full_Name);
                            Glass_Window glass_Window = new Glass_Window(description_Value, model_Value, model_Designation, height_Value, wight_Value, element_Group, full_Name, full_Name_Type);
                            Data_Parameter_On_Group_Stained_Glass_Windows.list_Group.Add(glass_Window);
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
                using (Transaction newT1 = new Transaction(Revit_Document_Parameter_On_Group_Stained_Glass_Windows.Document, "Выгрузка данных формата "))
                {
                    newT1.Start();
                    if (Data_Parameter_On_Group_Stained_Glass_Windows.error_Suppressio == true)
                    {
                        // Настройка для подавления предупреждений
                        FailureHandlingOptions failureOptions = newT1.GetFailureHandlingOptions();
                        failureOptions.SetFailuresPreprocessor(new IgnoreWarningPreprocessor());
                        newT1.SetFailureHandlingOptions(failureOptions);
                    }
                    List<Glass_Window> list = Data_Parameter_On_Group_Stained_Glass_Windows.list_Group.OrderBy(x => x.model_Value).ThenBy(x => double.Parse(x.height_Value)).ThenBy(x => double.Parse(x.wight_Value)).ToList();
                    int identical = 1;
                    for (int i = 0; i < list.Count; i++)
                    {
                        //Element element_Type = Revit_Document_Parameter_On_Group_Stained_Glass_Windows.Document.GetElement(Data_Parameter_On_Group_Stained_Glass_Windows.list_Group[i].element.GetTypeId());ыврыврывр
                        //Group group = Revit_Document_Parameter_On_Group_Stained_Glass_Windows.Document.GetElement(Revit_Document_Parameter_On_Group_Stained_Glass_Windows.UIDobument.Selection.PickObject(ObjectType.Element, "Выберите группу")) as Group;
                        Group element_Group = (Group)Revit_Document_Parameter_On_Group_Stained_Glass_Windows.Document.GetElement(list[i].element.Id);
                        GroupType groupType = element_Group.GroupType;
                        Element element_Type = Revit_Document_Parameter_On_Group_Stained_Glass_Windows.Document.GetElement(element_Group.GetTypeId()); 
                        bool iteration = false;
                        if (i < list.Count - 1)
                        {
                            
                            if (list[i].full_Name_Type.ToString() == list[i + 1].full_Name_Type.ToString() && iteration == false)
                            {
                                SSDK_Parameter.Set_Type_Name(groupType, list[i].full_Name_Type.ToString() + " (" + identical.ToString() + ")");
                                identical++;
                                iteration = true;
                            }
                            if (identical > 1 && list[i].full_Name_Type.ToString() != list[i + 1].full_Name_Type.ToString() && iteration == false)
                            {
                                SSDK_Parameter.Set_Type_Name(groupType, list[i].full_Name_Type.ToString() + " (" + identical.ToString() + ")");
                                identical = 1;
                                iteration = true;
                            }
                            if (i > 0 && identical == 1 && list[i].full_Name_Type.ToString() != list[i + 1].full_Name_Type.ToString() && list[i].full_Name_Type.ToString() == list[i - 1].full_Name_Type.ToString() && iteration == false)
                            {
                                SSDK_Parameter.Set_Type_Name(groupType, list[i].full_Name_Type.ToString() + " (" + identical.ToString() + ")");
                                iteration = true;
                            }
                            if (i == 0 && identical == 1 && list[i].full_Name_Type.ToString() != list[i + 1].full_Name_Type.ToString() && iteration == false)
                            {
                                SSDK_Parameter.Set_Type_Name(groupType, list[i].full_Name_Type.ToString());
                                iteration = true;
                            }
                            if (i > 0 && identical == 1 && list[i].full_Name_Type.ToString() != list[i + 1].full_Name_Type.ToString() && list[i].full_Name_Type.ToString() != list[i - 1].full_Name_Type.ToString() && iteration == false)
                            {
                                SSDK_Parameter.Set_Type_Name(groupType, list[i].full_Name_Type.ToString());
                                iteration = true;
                            }
                        }
                        else
                        {
                            if (identical > 1 && iteration == false)
                            {
                                SSDK_Parameter.Set_Type_Name(groupType, list[i].full_Name.ToString() + " (" + identical.ToString() + ")");
                                iteration = true;
                            }
                            if (identical == 1 && iteration == false)
                            {
                                SSDK_Parameter.Set_Type_Name(groupType, list[i].full_Name.ToString());
                                iteration = true;
                            }
                        }
                        Data_Parameter_On_Group_Stained_Glass_Windows.number_Elements++;
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

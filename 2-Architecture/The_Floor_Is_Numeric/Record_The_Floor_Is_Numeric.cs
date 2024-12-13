using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Windows.Controls;
using SSDK;

namespace WPFApplication.The_Floor_Is_Numeric
{
    public class Record_The_Floor_Is_Numeric
    {
        public void Record_The_Floor_Is_Numeric_Position()
        {
            using (Transaction newT2 = new Transaction(Revit_Document_The_Floor_Is_Numeric.Document, "Запись значения параметра"))
            {
                newT2.Start();
                if (Data_The_Floor_Is_Numeric.door_Checked)
                {
                    Record_Door_Window_Wall_Model_Group_The_Floor_Is_Numeric(BuiltInCategory.OST_Doors);
                }
                if (Data_The_Floor_Is_Numeric.window_Checked)
                {
                    Record_Door_Window_Wall_Model_Group_The_Floor_Is_Numeric(BuiltInCategory.OST_Windows);
                }
                if (Data_The_Floor_Is_Numeric.wall_Checked)
                {
                    Record_Door_Window_Wall_Model_Group_The_Floor_Is_Numeric(BuiltInCategory.OST_Walls);
                }
                if (Data_The_Floor_Is_Numeric.model_Group_Checked)
                {
                    Record_Door_Window_Wall_Model_Group_The_Floor_Is_Numeric(BuiltInCategory.OST_IOSModelGroups);
                }
                if (Data_The_Floor_Is_Numeric.Room_Checked)
                {
                    Record_Door_Window_Wall_Model_Group_The_Floor_Is_Numeric(BuiltInCategory.OST_Rooms);
                }
                if (Data_The_Floor_Is_Numeric.Floor_Checked)
                {
                    Record_Door_Window_Wall_Model_Group_The_Floor_Is_Numeric(BuiltInCategory.OST_Floors);
                }
                newT2.Commit();
            }
        }
        public void Record_Door_Window_Wall_Model_Group_The_Floor_Is_Numeric(BuiltInCategory category)
        {
            try
            {
                FilteredElementCollector window = new FilteredElementCollector(Revit_Document_The_Floor_Is_Numeric.Document);
                ICollection<Element> all_Elements = window.OfCategory(category).WhereElementIsNotElementType().ToElements();
                ICollection<Element> all_Elements_Not_Context = new List<Element>();
                foreach (Element element_Not_Context in all_Elements)
                {
                    if(element_Not_Context.get_Parameter(BuiltInParameter.ELEM_CATEGORY_PARAM).AsValueString() == "Стены")
                    {  
                        if(element_Not_Context.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsValueString() == "Базовая стена"
                        || element_Not_Context.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsValueString() == "Витраж")
                        {
                            all_Elements_Not_Context.Add(element_Not_Context);
                        }
                        else
                        {
                            
                        }
                    }
                    else
                    {
                        all_Elements_Not_Context.Add(element_Not_Context);
                    }
                }
                all_Elements = all_Elements_Not_Context;
                foreach (Element element in all_Elements)
                {
                    bool element_true = false;
                    foreach (string str in Data_The_Floor_Is_Numeric.work_Set_Igonre_Collection.Items)
                    {
                        string parameter_Value = element.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM).AsValueString();
                        if (parameter_Value == str)
                        {
                            element_true = true;
                        }
                    }
                    if (!element_true)
                    {
                        Element level_ID = Revit_Document_The_Floor_Is_Numeric.Document.GetElement(element.LevelId);
                        Parameter parameter_Level = level_ID.get_Parameter(Data_The_Floor_Is_Numeric.parameten_Guid);
                        Parameter parameter_Element = element.get_Parameter(Data_The_Floor_Is_Numeric.parameten_Guid);
                        parameter_Element.Set(Convert.ToDouble(parameter_Level.AsValueString()));
                        Data_The_Floor_Is_Numeric.number_True_Element++;
                    }
                }
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
        }
        public void Work_Set_Download_The_Floor_Is_Numeric()
        {
            try
            {
                var worksetTable = new FilteredWorksetCollector(Revit_Document_The_Floor_Is_Numeric.Document).OfKind(WorksetKind.UserWorkset).ToList();
            ListView all_Elements = new ListView();
            foreach (Workset workset in worksetTable)
            {
                all_Elements.Items.Add(workset.Name);
            }
            if (all_Elements.Items.Count > 0)
            {
                Data_The_Floor_Is_Numeric.work_Set_Collection = all_Elements;
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

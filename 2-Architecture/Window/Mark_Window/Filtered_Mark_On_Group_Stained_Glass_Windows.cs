using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SSDK;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApplication.The_Floor_Is_Numeric;

namespace WPFApplication.Mark_Window
{
    public class Filtered_Mark_On_Group_Stained_Glass_Windows
    {
        public Filtered_Mark_On_Group_Stained_Glass_Windows()
        {
            try
            {
                FilteredElementCollector window = new FilteredElementCollector(Revit_Document_Mark_On_Group_Stained_Glass_Windows.Document);
                ICollection<Element> all_Elements = window.OfCategory(BuiltInCategory.OST_IOSModelGroups).WhereElementIsNotElementType().ToElements();
                foreach (Group element in all_Elements)
                {
                   
                    bool collection_All_Grop_In_Model_True = false;
                    Element element_Type = Revit_Document_Mark_On_Group_Stained_Glass_Windows.Document.GetElement(element.GetTypeId());
                    double parameter_Value = element_Type.get_Parameter(Data_Mark_On_Group_Stained_Glass_Windows.guid_Group).AsDouble() * 304.8;
                    if (211 < parameter_Value && parameter_Value < 212.999)
                    {
                        collection_All_Grop_In_Model_True = true;
                    }
                    if (collection_All_Grop_In_Model_True)
                    {
                        Data_Mark_On_Group_Stained_Glass_Windows.filtered_Group.Add(element); 
                    }

                }
                Data_Mark_On_Group_Stained_Glass_Windows.filtered_Group = Data_Mark_On_Group_Stained_Glass_Windows.filtered_Group.GroupBy(p => p.GetTypeId()).Select(g => g.First()).ToList();
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
        }
    }
}

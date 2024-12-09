using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApplication.The_Floor_Is_Numeric;

namespace WPFApplication.Parameter_On_Group_Stained_Glass_Windows
{
    public class Filtered_Mark_On_Group_Stained_Glass_Windows
    {
        public Filtered_Mark_On_Group_Stained_Glass_Windows()
        {
            try
            {
                FilteredElementCollector window = new FilteredElementCollector(Revit_Document_Parameter_On_Group_Stained_Glass_Windows.Document);
                ICollection<Element> all_Elements = window.OfCategory(BuiltInCategory.OST_IOSModelGroups).WhereElementIsNotElementType().ToElements();
                foreach (Group element in all_Elements)
                {
                    ICollection<ElementId> collection_ElementId_All_Grop_In_Model = element.GetMemberIds();
                    List<Element> collection_Element_All_Grop_In_Model = new List<Element>();
                    foreach (ElementId elementId in collection_ElementId_All_Grop_In_Model) 
                    {
                        collection_Element_All_Grop_In_Model.Add(Revit_Document_Parameter_On_Group_Stained_Glass_Windows.Document.GetElement(elementId));
                    }
                    bool collection_All_Grop_In_Model_True = false;
                    foreach (Element element_Id_In_Goup in collection_Element_All_Grop_In_Model)
                    {
                        if (element_Id_In_Goup.Category != null&&element_Id_In_Goup.Category.Name.ToString() == "Стены")
                        {
                            Element element_Type = Revit_Document_Parameter_On_Group_Stained_Glass_Windows.Document.GetElement(element_Id_In_Goup.GetTypeId());
                            double parameter_Value = element_Type.get_Parameter(Data_Parameter_On_Group_Stained_Glass_Windows.guid_Group).AsDouble()*304.8;
                            if (211 < parameter_Value && parameter_Value < 212.999)
                            {
                                collection_All_Grop_In_Model_True = true;
                            }
                        }
                    }
                    if (collection_All_Grop_In_Model_True)
                    {
                        Data_Parameter_On_Group_Stained_Glass_Windows.filtered_Group.Add(element); 
                    }
                }
                Data_Parameter_On_Group_Stained_Glass_Windows.filtered_Group = Data_Parameter_On_Group_Stained_Glass_Windows.filtered_Group.GroupBy(p => p.GetTypeId()).Select(g => g.First()).ToList();
                }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
    }
}

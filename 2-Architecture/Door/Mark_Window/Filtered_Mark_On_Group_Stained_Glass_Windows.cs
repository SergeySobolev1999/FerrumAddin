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


namespace WPFApplication.Mark_Door
{
    public class Filtered_Mark_Window
    {
        public Filtered_Mark_Window()
        {
            try
            {
                FilteredElementCollector window = new FilteredElementCollector(Revit_Document_Mark_Window.Document);
                ICollection<Element> all_Elements = window.OfCategory(BuiltInCategory.OST_Doors).WhereElementIsNotElementType().ToElements();
                foreach (Element element in all_Elements)
                {
                    Element element_Type = Revit_Document_Mark_Window.Document.GetElement(element.GetTypeId());
                    if (Revit_Document_Mark_Window.Document.GetElement(element.GetTypeId()) != null&& element_Type.get_Parameter(Data_Mark_Door.guid_COD)!=null)
                    {
                        
                        double parameter_Value = element_Type.get_Parameter(Data_Mark_Door.guid_COD).AsDouble() * 304.8;
                        if (206.999 < parameter_Value && parameter_Value < 209 && element_Type.LookupParameter("ЮТС_Dynamo_ID") != null)
                        {
                            Data_Mark_Door.filtered_Group.Add(element);
                        }
                    }
                }
                
                Data_Mark_Door.filtered_Group = Data_Mark_Door.filtered_Group.GroupBy(p => p.GetTypeId()).Select(g => g.First()).ToList();
                
            }
            catch (Exception ex) 
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
        }
    }
}

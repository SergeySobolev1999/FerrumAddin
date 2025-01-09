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

namespace WPFApplication.Parameter_Window
{
    public class Filtered_Mark_Window
    {
        public Filtered_Mark_Window()
        {
            try
            {
                FilteredElementCollector window = new FilteredElementCollector(Revit_Document_Parameter_Window.Document);
                ICollection<Element> all_Elements = window.OfCategory(BuiltInCategory.OST_Windows).WhereElementIsNotElementType().ToElements();
                foreach (Element element in all_Elements)
                {
                    Element element_Type = Revit_Document_Parameter_Window.Document.GetElement(element.GetTypeId());
                    if (Revit_Document_Parameter_Window.Document.GetElement(element.GetTypeId()) != null&& element_Type.get_Parameter(Data_Parameter_Window.guid_COD)!=null)
                    {
                        
                        double parameter_Value = element_Type.get_Parameter(Data_Parameter_Window.guid_COD).AsDouble() * 304.8;
                        if (209 < parameter_Value && parameter_Value < 210.999 && element_Type.LookupParameter("ЮТС_Dynamo_ID") != null)
                        {
                            Data_Parameter_Window.filtered_Group.Add(element_Type);
                        }
                    }
                }
                Data_Parameter_Window.filtered_Group = Data_Parameter_Window.filtered_Group.GroupBy(p => p.Id).Select(g => g.First()).ToList();
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
        }
    }
}

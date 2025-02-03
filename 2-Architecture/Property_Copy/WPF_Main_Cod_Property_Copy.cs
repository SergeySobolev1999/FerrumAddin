using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using WPFApplication.Property_Copy;

namespace WPFApplication.Property_Copy
{
    
    public class WPF_Main_Cod_Property_Copy
    {
       
    }
    public class Pick_Element
    {
        public Element Pick_Element_Donor()
        {
            Selection choices = Document_Property_Copy.UIDobument.Selection;
            ISelectionFilter filter = new MassSelectionFilter();
            Reference has_Pick_One = choices.PickObject(ObjectType.Element, filter);
            if (has_Pick_One != null)
            {
                return Document_Property_Copy.Document.GetElement(has_Pick_One) ;
            }
            return null;
        }
    }
    public class MassSelectionFilter : ISelectionFilter
    {
        private HashSet<int> categories_Filtered;
        public MassSelectionFilter()
        {
            List<BuiltInCategory> categories = new List<BuiltInCategory> {
                BuiltInCategory.OST_Doors,
                BuiltInCategory.OST_Windows};
            categories_Filtered = new HashSet<int>(categories.Select(c => (int)c));
        }
        public bool AllowElement(Element element)
        {
            double zh_Cod = 0;
            if (Document_Property_Copy.Document.GetElement(element.GetTypeId()) != null)
            {
                Element element_Type = Document_Property_Copy.Document.GetElement(element.GetTypeId());
                if (element_Type.get_Parameter(Data_Class_Property_Copy.zh_Cod).AsValueString() != null)
                {
                    zh_Cod = element_Type.get_Parameter(Data_Class_Property_Copy.zh_Cod).AsDouble() * 304.8;
                }
            }
            if (element.Category != null && categories_Filtered != null && 206.999 < zh_Cod && zh_Cod < 210.999)
            {
                return true;
            }
            return false;
        }
        public bool AllowReference(Reference refer, XYZ point)
        {
            return false;
        }
    }
    
    

   
    
}

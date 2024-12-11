using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RevitAPI_Basic_Course.Creating_Specifications
{
    public class Shedule_Elements_Creating_Specifications
    {
        public void Shedule_Elements_Creating_Specifications_List()
        {
            try
            {
                FilteredElementCollector collector = new FilteredElementCollector(Revit_Document_Creating_Specifications.Document);
            Data_Creating_Specifications.shedule_Elements = collector.OfCategory(BuiltInCategory.OST_TitleBlocks).ToElements();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
    }
}

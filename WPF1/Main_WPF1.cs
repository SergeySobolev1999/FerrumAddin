using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using FerrumAddin.Main_WPF1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerrumAddin
{
    [Transaction(TransactionMode.Manual)]
    public class WPF1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                // dockable window id
                Main_WPF1_ViewModel wPF = new Main_WPF1_ViewModel();
                wPF.Show();
            }
            catch (Exception ex)
            {
                // show error info dialog
                TaskDialog.Show("Info Message", ex.Message);
            }
            // return result
            return Result.Succeeded;
        }
    }
}

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WPFApplication.Mark_Window;


namespace WPFApplication.Mark_Door
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class Main_Mark_Window : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Revit_Document_Mark_Window.Initialize(commandData);
            WPF_Main_MarkWindow wPF_Main_MarkWindow = new WPF_Main_MarkWindow();    
            wPF_Main_MarkWindow.ShowDialog();
            return Result.Succeeded;
        }
    }
}

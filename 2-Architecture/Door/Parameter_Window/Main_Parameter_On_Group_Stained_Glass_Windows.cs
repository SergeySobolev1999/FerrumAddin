using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApplication.Mark_Window;
using WPFApplication.Parameter_Window;
using WPFApplication.The_Floor_Is_Numeric;

namespace WPFApplication.Parameter_Door
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class Main_Parameter_Window : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Revit_Document_Parameter_Window.Initialize(commandData);
            WPF_Main_Parameter_Door wPF_Main_Parameter_Door = new WPF_Main_Parameter_Door();
            wPF_Main_Parameter_Door.ShowDialog();
            return Result.Succeeded;
        }
    }
}

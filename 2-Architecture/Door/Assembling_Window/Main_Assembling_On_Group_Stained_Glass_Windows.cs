using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WPFApplication.Assembling_Door
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class Main_Assembling_Window : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                //sdg
                Revit_Document_Assembling_Window.Initialize(commandData);
                WPF_Main_Assembling_Door wPF_Main_Assembling_Window = new WPF_Main_Assembling_Door();
                wPF_Main_Assembling_Window.ShowDialog();
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
            return Result.Succeeded;
        }
    }
}

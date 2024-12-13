using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApplication.The_Floor_Is_Numeric
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class Main_The_Floor_Is_Numeric : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                WPF_Main_The_Floor_Is_Numeric wpf_Main_The_Floor_Is_Numeric = new WPF_Main_The_Floor_Is_Numeric(commandData);
            wpf_Main_The_Floor_Is_Numeric.ShowDialog();
            return Result.Succeeded;
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
                return Result.Succeeded;
            }

        }

    }
}

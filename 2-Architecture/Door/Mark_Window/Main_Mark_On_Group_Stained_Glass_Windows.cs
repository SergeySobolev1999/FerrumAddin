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
using WPFApplication.Licenses;
using WPFApplication.Mark_Window;
using WPFApplication.The_Floor_Is_Numeric;


namespace WPFApplication.Mark_Door
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class Main_Mark_Window : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Revit_Document_Mark_Window.Initialize(commandData);
            SSDK_Data.username = Environment.UserName;
            if (SSDK_Data.licenses_Connection)
            {
                WPF_Main_MarkWindow wPF_Main_MarkWindow = new WPF_Main_MarkWindow();
                wPF_Main_MarkWindow.ShowDialog();
            }
            else
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. Ваша лицензия недоступна. Выполните переподключение");
                s_Mistake_String.ShowDialog();
            }
            return Result.Succeeded;
        }
    }
}

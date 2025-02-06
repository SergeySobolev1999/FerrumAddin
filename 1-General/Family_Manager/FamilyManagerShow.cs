using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using SSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApplication.Licenses;
using WPFApplication.The_Floor_Is_Numeric;

namespace FerrumAddin
{

    [Transaction(TransactionMode.Manual)]
    public class FamilyManagerShow : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                if (1 == 1)
                {
                    SSDK_Data.username = Environment.UserName;
                    if (SSDK_Data.licenses_Connection)
                    {
                        // dockable window id
                        DockablePaneId id = new DockablePaneId(new Guid("{68D44FAC-CF09-46B2-9544-D5A3F809373C}"));
                        DockablePane dockableWindow = commandData.Application.GetDockablePane(id);
                        dockableWindow.Show();
                    }
                    else
                    {
                        S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. Ваша лицензия недоступна. Выполните переподключение");
                        s_Mistake_String.ShowDialog();
                    }
                }
                else
                {
                    S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. Плагин в разработке");
                    s_Mistake_String.ShowDialog();
                }
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

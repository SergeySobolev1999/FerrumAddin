using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApplication.newProperty_Copy
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class newMainMaterialProperty_Copy : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            SSDK_Data.licenses_Name = Environment.UserName;
            if (SSDK_Data.licenses_Connection)
            {
                var viewModel = new newViewModelProperty_Copy(commandData);
                var window = new newWPF_Main_Property_Copy(commandData)
                {
                    DataContext = viewModel
                };
                var revitHandle = Autodesk.Windows.ComponentManager.ApplicationWindow;
                var helper = new System.Windows.Interop.WindowInteropHelper(window)
                {
                    Owner = revitHandle
                };
                window.Show();
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

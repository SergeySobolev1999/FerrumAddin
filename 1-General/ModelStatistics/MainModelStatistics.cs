using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using WPFApplication.newProperty_Copy;

namespace FerrumAddin.ModelStatistics
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class MainModelStatistics : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            SSDK_Data.licenses_Name = Environment.UserName;
            //if (SSDK_Data.licenses_Connection)
            //{
                var viewModel = new StatisticsViewModel(commandData);
            var window = new WpfMainVeiw(viewModel);
                window.Show();
            //}
            //else
            //{
            //    S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. Ваша лицензия недоступна. Выполните переподключение");
            //    s_Mistake_String.ShowDialog();
            //}
            return Result.Succeeded;
        }
    }
}

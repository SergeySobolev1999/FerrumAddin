using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using FerrumAddin;
using SSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApplication.Rooms
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class MainRooms : IExternalCommand
    {
        public static UIApplication UiApp { get; private set; }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UiApp = commandData.Application;
            SSDK_Data.licenses_Name = Environment.UserName;
            if (SSDK_Data.licenses_Connection)
            {
                // Сохраняем ссылку на UIApplication
                var uiApp = commandData.Application;

                // Получаем заранее зарегистрированную панель
                var paneId = new DockablePaneId(new Guid("E2C3D2B1-6F24-4B3C-AF2A-CEFF88D39C00"));
                DockablePane pane;

                try
                {
                    pane = uiApp.GetDockablePane(paneId);
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Ошибка", $"Не удалось получить панель: {ex.Message}");
                    return Result.Failed;
                }

                //Инициализируем ViewModel(сохраняется в App при старте)
                var vm = App.ViewModel;
                if (vm == null)
                {
                    TaskDialog.Show("Ошибка", "ViewModel не инициализирован. Проверь регистрацию панели в OnStartup.");
                    return Result.Failed;
                }

                vm.Initialize(uiApp); // п

                try
                {
                    pane.Show();
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Ошибка", $"Не удалось отобразить панель: {ex.Message}");
                    return Result.Failed;
                }

                return Result.Succeeded;

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

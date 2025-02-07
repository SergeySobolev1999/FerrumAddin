using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApplication.Property_Copy
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class Main_Class_Property_Copy : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                if (1 ==2)
                {
                    //SSDK_Data.username = Environment.UserName;
                    //if (SSDK_Data.licenses_Connection)
                    //{
                    int a = 0;
                    WPF_Main_Property_Copy wPF_Main_Property_Copy = new WPF_Main_Property_Copy(commandData);
                    wPF_Main_Property_Copy.Show();
                    //}
                    //else
                    //{
                    //    S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. Ваша лицензия недоступна. Выполните переподключение");
                    //    s_Mistake_String.ShowDialog();
                    //}
                }
                else
                {
                    S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. Плагин в разработке");
                    s_Mistake_String.ShowDialog();
                }
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

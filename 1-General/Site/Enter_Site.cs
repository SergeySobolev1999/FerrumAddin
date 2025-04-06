using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using FerrumAddin._1_General.Site;
using SSDK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApplication.Enter_Site
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class Enter_Site : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                SSDK_Data.licenses_Name = Environment.UserName;
                if (SSDK_Data.licenses_Connection)
                {
                    Document_Site.Initialize(commandData);
                    string url = "https://bim.zhcom.ru/";
                    using (Transaction newT1 = new Transaction(Document_Site.Document, "Переход на сайт"))
                    {
                        newT1.Start();
                        if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                        {
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = url,
                                UseShellExecute = true
                            });
                        }
                        newT1.Commit();
                        return Result.Succeeded;
                    }
                }
                else
                {
                    S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. Ваша лицензия недоступна. Выполните переподключение");
                    s_Mistake_String.ShowDialog();
                }
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

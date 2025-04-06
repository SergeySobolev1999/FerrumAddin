using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApplication.Licenses;
using WPFApplication.The_Floor_Is_Numeric;


namespace WPFApplication.Assembling_Project_On_Group_Stained_Glass_Windows
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class Main_Assembling_On_Group_Stained_Glass_Windows : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
              
                    Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Initialize(commandData);
                SSDK_Data.licenses_Name = Environment.UserName;
                if (SSDK_Data.licenses_Connection)
                {
                    WPF_Main_Assembling_On_Group_Stained_Glass_Windows wPF_On_Group_Stained_Glass_Windows = new WPF_Main_Assembling_On_Group_Stained_Glass_Windows();
                    wPF_On_Group_Stained_Glass_Windows.ShowDialog();
                }
                else
                {
                    S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. Ваша лицензия недоступна. Выполните переподключение");
                    s_Mistake_String.ShowDialog();
                }
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

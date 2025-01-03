using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApplication.Mark_Window;
using WPFApplication.The_Floor_Is_Numeric;

namespace WPFApplication.Parameter_Door
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class Main_Parameter_Window : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                Revit_Document_Parameter_Window.Initialize(commandData);
                Data_Parameter_Window.number_Elements = 0;
                Data_Parameter_Window.filtered_Group.Clear();
                Data_Parameter_Window.list_Group.Clear();
                Filtered_Mark_Window filtered_Mark_On_Group_Stained_Glass_Windows = new Filtered_Mark_Window(); 
                Collecting_Windows collecting_Windows = new Collecting_Windows();
                if (Data_Parameter_Window.iteration_Recaive_Value_In_Parameter == true)
                {

                    S_Mistake_String s_Mistake_String_Warning = new S_Mistake_String(Data_Parameter_Window.iteration_Recaive_Value_In_Parameter_Watringn);
                    s_Mistake_String_Warning.ShowDialog();
                }
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Запись завершена. Успешно обработаных элементов: " + Data_Parameter_Window.number_Elements.ToString());
                s_Mistake_String.ShowDialog();
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

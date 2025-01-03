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
using WPFApplication.Parameter_Window;
using WPFApplication.The_Floor_Is_Numeric;

namespace WPFApplication.Mark_Door
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class Main_Mark_Window : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                Revit_Document_Mark_Window.Initialize(commandData);
                Data_Mark_Window.filtered_Group.Clear();
                Data_Mark_Window.list_Group.Clear();
                Filtered_Mark_Window filtered_Mark_Window = new Filtered_Mark_Window();
                Collecting_Window collecting_Group_Stained_Glass_Windows = new Collecting_Window(); 
                Sort_On_Mark_Window sort_On_Mark_Window = new Sort_On_Mark_Window();
                if(Data_Mark_Window.iteration_Recaive_Value_In_Parameter==true)
                {
                    
                    S_Mistake_String s_Mistake_String_Warning = new S_Mistake_String(Data_Mark_Window.iteration_Recaive_Value_In_Parameter_Watringn);
                    s_Mistake_String_Warning.ShowDialog();
                }
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Запись завершена. Успешно обработаных элементов: " + (Data_Mark_Window.number_Elements).ToString());
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

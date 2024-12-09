using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApplication.The_Floor_Is_Numeric;

namespace WPFApplication.Parameter_On_Group_Stained_Glass_Windows
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class Main_Parameter_On_Group_Stained_Glass_Windows : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                Revit_Document_Parameter_On_Group_Stained_Glass_Windows.Initialize(commandData);
                Data_Parameter_On_Group_Stained_Glass_Windows.number_Elements = 0;
                Data_Parameter_On_Group_Stained_Glass_Windows.filtered_Group.Clear();
                Data_Parameter_On_Group_Stained_Glass_Windows.list_Group.Clear();
                Filtered_Mark_On_Group_Stained_Glass_Windows filtered_Mark_On_Group_Stained_Glass_Windows = new Filtered_Mark_On_Group_Stained_Glass_Windows();
                Collecting_Group_Stained_Glass_Windows collecting_Parameters_On_Group_Stained_Glass_Windows = new Collecting_Group_Stained_Glass_Windows();
                Sort_On_Parameter sort_On_Parameter = new Sort_On_Parameter();
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Запись завершена. Успешно обработаных элементов: " + Data_Parameter_On_Group_Stained_Glass_Windows.number_Elements.ToString());
                s_Mistake_String.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return Result.Succeeded;
        }
    }
}

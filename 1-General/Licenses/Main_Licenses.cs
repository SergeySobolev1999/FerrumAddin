using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApplication.Licenses
{
    public class Main_Assembling_Window 
    {
        public Main_Assembling_Window()
        {
            try
            {
                WPF_Main_Licenses wPF_Main_Licenses = new WPF_Main_Licenses();
                wPF_Main_Licenses.ShowDialog();
               
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
        }
    }
}

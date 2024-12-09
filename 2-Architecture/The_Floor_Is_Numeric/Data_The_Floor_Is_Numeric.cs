using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPFApplication.The_Floor_Is_Numeric
{
    public class Data_The_Floor_Is_Numeric
    {
        public static bool door_Checked { get; set; } = false;
        public static bool window_Checked { get; set; } = false;
        public static bool wall_Checked { get; set; } = false;
        public static bool model_Group_Checked { get; set; } = false;
        public static bool Room_Checked { get; set; } = false;
        public static bool Floor_Checked { get; set; } = false;
        public static ListView work_Set_Collection { get; set; } = new ListView();
        public static ListView work_Set_Igonre_Collection { get; set; } = new ListView();
        public static int number_True_Element { get; set; } = 0;

        public static Guid parameten_Guid { get; set; } =  new Guid("cd5b55ea-65d7-46b9-97f6-d200abb81063");
       
    }
    public static class Revit_Document_The_Floor_Is_Numeric
    {
        public static UIApplication UIApplication { get; set; }
        public static UIDocument UIDobument { get => UIApplication.ActiveUIDocument; }
        public static Document Document { get => UIDobument.Document; }
        public static ExternalCommandData Data { get; set; }
        public static void Initialize(ExternalCommandData commandData)
        {
            Data = commandData;
            UIApplication = commandData.Application;
        }
    }
}

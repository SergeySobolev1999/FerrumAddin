using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApplication.Property_Copy
{
    public class Data_Class_Property_Copy
    {
        public static Guid zh_Cod { get; set; } = new Guid("631cd69e-065f-4ec2-8894-4359325312c3");
        public static FamilyInstance element_Donor { get; set; } = null;
    }
    public static class Document_Property_Copy
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

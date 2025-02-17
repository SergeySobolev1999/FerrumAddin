using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApplication.Parameter_Door;

namespace SSDK
{
    public class SSDK_Set
    {
        public static bool Set_Parameter(Parameter parameter, string value)
        {
            if (parameter == null) { return false; }
            //else if (parameter.AsValueString() == null) { return false; }
            else if (parameter.AsValueString() == value) { return false; }
            else { parameter.Set(value); }
            return true;
        }
        public static bool Set_Type_Name(Element element, string value)
        {
            if (element == null) { return false; }
            //else if (element.Name == null) { return false; }
            else if (element.Name == value) { return false; }
            else { element.Name = value; }
            return true;
        }
    }
}

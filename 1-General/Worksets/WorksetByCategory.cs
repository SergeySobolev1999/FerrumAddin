
using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace WPFApplication.Worksets
{
    [Serializable]
    public class WorksetByCategory : WorksetBy
    {
        public List<BuiltInCategory> revitCategories;

        public WorksetByCategory()
        {

        }
    }
}

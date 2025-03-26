using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace WPFApplication.Worksets
{
    public static class WorksetTool
    {
        public static List<string> GetEmptyWorksets(Document doc)
        {
            List<string> wnames = new List<string>();
            if (!doc.IsWorkshared) return null;

            List<Autodesk.Revit.DB.Workset> wids = new FilteredWorksetCollector(doc)
                .OfKind(WorksetKind.UserWorkset)
                .ToWorksets()
                .ToList();

            foreach (Autodesk.Revit.DB.Workset w in wids)
            {
                ElementWorksetFilter wfilter = new ElementWorksetFilter(w.Id);
                FilteredElementCollector col = new FilteredElementCollector(doc).WherePasses(wfilter);
                if(col.GetElementCount() == 0)
                {
                    wnames.Add(w.Name);
                }
            }
            return wnames;
        }
    }
}

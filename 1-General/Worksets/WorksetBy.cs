﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Autodesk.Revit.DB;

namespace WPFApplication.Worksets
{
    public abstract class WorksetBy
    {
        public string WorksetName;

        public Autodesk.Revit.DB.Workset GetWorkset(Document doc)
        {
            Autodesk.Revit.DB.Workset wset = WorksetBy.GetOrCreateWorkset(doc, WorksetName);
            return wset;
        }

        public static Autodesk.Revit.DB.Workset GetOrCreateWorkset(Document doc, string worksetName)
        {
            IList<Autodesk.Revit.DB.Workset> userWorksets = new FilteredWorksetCollector(doc)
                .OfKind(WorksetKind.UserWorkset)
                .ToWorksets();

            bool checkNotExists = WorksetTable.IsWorksetNameUnique(doc, worksetName);

            if (!checkNotExists)
            {
                Debug.WriteLine("Workset exists: " + worksetName);
                Autodesk.Revit.DB.Workset wset = new FilteredWorksetCollector(doc)
                .OfKind(WorksetKind.UserWorkset)
                .ToWorksets()
                .Where(w => w.Name == worksetName)
                .First();
                return wset;
            }
            else
            {
                Debug.WriteLine("Create workset: " + worksetName);
                Autodesk.Revit.DB.Workset wset = Autodesk.Revit.DB.Workset.Create(doc, worksetName);
                return wset;
            }
        }


        public static void SetWorkset(Element elem, Autodesk.Revit.DB.Workset w)
        {
            Debug.WriteLine("Set workset: " + w.Name + " for elem id " + elem.Id.IntegerValue);

            bool elemNonGroup = (elem.GroupId == null) || (elem.GroupId == ElementId.InvalidElementId);

            if (elemNonGroup)
            {
                Parameter wsparam = elem.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM);
                if (wsparam == null)
                {
                    Debug.WriteLine("Invalid workset parameter");
                    return;
                }
                if (wsparam.IsReadOnly)
                {
                    Debug.WriteLine("Workset parameter is readonly, skip");
                    return;
                }

                wsparam.Set(w.Id.IntegerValue);
                Debug.WriteLine("Set workset success");
            }
            else
            {
                Group gr = elem.Document.GetElement(elem.GroupId) as Group;
                Debug.WriteLine("Elem is in group; set workset for the group: " + gr.Name);
                SetWorkset(gr, w);
            }
        }
    }
}

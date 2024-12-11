using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApplication.SharedParameterManager;

namespace RevitAPI_Basic_Course.Creating_Specifications
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class Main_Creating_Specifications : IExternalCommand
    {
        public static bool iteration_Export = false;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Folder folder = new Folder();

             //var viewModel = new Duplicate_Sheet_Delete_Creating_Specification();
             ////var view = new Duplicate_Sheet_Main_Creating_Specifications(viewModel);
             //Duplicate_Sheet_Delete_Creating_Specification duplicate_Sheet_Delete_Creating_Specification = new Duplicate_Sheet_Delete_Creating_Specification();
             //duplicate_Sheet_Delete_Creating_Specification.Duplicate_Sheet_Delete_Creating_Specification_Filtered();
             //if (Data_Creating_Specifications.sheet_Collection_Filtered.Count > 0)
             //{
             //    view.ShowDialog();
             //}

             WPF_Creating_Specifications wpf_Creating_Specifications = new WPF_Creating_Specifications(commandData);
            wpf_Creating_Specifications.ShowDialog();
            return Result.Succeeded;

        }

    }
}

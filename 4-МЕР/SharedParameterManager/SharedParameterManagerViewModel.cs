using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RevitAPI_Basic_Course.Creating_Specifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApplication.SharedParameterManager
{
    public partial class SharedParameterManagerViewModel : ObservableObject
    {
        [ObservableProperty] private ObservableCollection<SharedParameterDescriptor> _parameters = [];

        public SharedParameterManagerViewModel()
        {
            try
            {

                //var parameters = new FilteredElementCollector(RevitAPI.Document).
                //    OfClass(typeof(SharedParameterElement)).
                //    Cast<SharedParameterElement>().
                //    ToArray();
                var parameters = new FilteredElementCollector(Revit_Document_Creating_Specifications.Document).OfCategory(BuiltInCategory.OST_Sheets).WhereElementIsNotElementType().ToElements();

                foreach (ViewSheet parameter in parameters)
                {
                    //Guid guid = new Guid("a85b7661-26b0-412f-979c-66af80b4b2c3");
                    //string str = parameter.get_Parameter(guid).AsString();

                    if (parameter != null && parameter.SheetNumber.Contains("‏‏‎ ‎"))
                    {
                        Parameters.Add(new SharedParameterDescriptor(parameter));
                        Data_Creating_Specifications.sheet_Collection_Filtered.Add(parameter);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        [RelayCommand]
        private void DeleteParameter(SharedParameterDescriptor parameter)
        {
            try
            {

                using var transaction2 = new Transaction(Revit_Document_Creating_Specifications.Document, $"Составление списка удаляемых листов: {parameter.Name}");
                transaction2.Start();

                Data_Creating_Specifications.list_Elements.Add(parameter);


                Parameters.Remove(parameter);


                transaction2.Commit();

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        public void Delete_All_Parameter_True()
        {
            try
            {
                using var transaction2 = new Transaction(Revit_Document_Creating_Specifications.Document, $"Составление списка удаляемых листов:");
                transaction2.Start();
                //for (int i = 0; i < Parameters.Count; i++)
                //{
                //    Data_Creating_Specifications.list_Elements.Add(Parameters[i]);
                //}
                
                transaction2.Commit();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
    }
}

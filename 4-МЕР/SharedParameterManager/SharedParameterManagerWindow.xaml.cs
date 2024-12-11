using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitAPI_Basic_Course.Creating_Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFApplication.SharedParameterManager
{
    /// <summary>
    /// Логика взаимодействия для SharedParameterManagerWindow.xaml
    /// </summary>
    public partial class SharedParameterManagerWindow : Window
    {
        public SharedParameterManagerWindow(SharedParameterManagerViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (Transaction newT1 = new Transaction(Revit_Document_Creating_Specifications.Document, "Удаление листов: "))
            {
                newT1.Start();
                if (Data_Creating_Specifications.list_Elements.Count > 0)
                {
                    ICollection<ElementId> elementId = new List<ElementId>();
                    for (int i = 0; i < Data_Creating_Specifications.list_Elements.Count; i++)
                    {
                        var parameters2 = new FilteredElementCollector(Revit_Document_Creating_Specifications.Document).OfCategory(BuiltInCategory.OST_Sheets).WhereElementIsNotElementType().ToElements();
                        bool el = false;
                        foreach (ViewSheet parameter1 in parameters2)
                        {
                            if (parameter1 != null && parameter1.Id == Data_Creating_Specifications.list_Elements[i].Id)
                            {
                                el = true;
                            }
                        }
                        elementId.Add(Data_Creating_Specifications.list_Elements[i].Id);
                      
                    }
                    
                    Revit_Document_Creating_Specifications.Document.Delete(elementId);
                    
                }
                newT1.Commit();
            }
            Close();
        }
        private void close_Click(object sender, RoutedEventArgs e)
        {
            Data_Creating_Specifications.list_Filter_Bool = false;
            Close();
        }
        private void Label_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void Delete_All_Parameter(object sender, RoutedEventArgs e)
        {
            foreach(SharedParameterDescriptor element in table_Parameters.Items)
            {
                Data_Creating_Specifications.list_Elements.Add(element);
            }
            table_Parameters.ItemsSource = null;
        }
    }
}

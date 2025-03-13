using Autodesk.Revit.UI;
using SSDK;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFApplication.newMaterial_Select_Element_Application
{
    /// <summary>
    /// Логика взаимодействия для newWPFMainMaterialSelectElementApplication.xaml
    /// </summary>
    public partial class newWPFMainMaterialSelectElementApplication : Window
    {
        public newWPFMainMaterialSelectElementApplication(ExternalCommandData commandData)
        {
            InitializeComponent();
            var viewModel = new newViewModelMaterialSelectElementApplication(commandData);
            Version.Text = SSDK_Data.plugin_Version;
            viewModel.CloseAction = () => this.Close();
            DataContext = viewModel;
        }
        private void Label_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void closeWPF_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Закрытие через стандартный обработчик
        }
        private void ListBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox == null)
                return;

            var border = listBox.Parent as Border;
            if (border != null)
            {
                e.Handled = true;
                var eventArgs = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                {
                    RoutedEvent = UIElement.MouseWheelEvent,
                    Source = sender
                };
                border.RaiseEvent(eventArgs);
            }
        }
        
    }
}    

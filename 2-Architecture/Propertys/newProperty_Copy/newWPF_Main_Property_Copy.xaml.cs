using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SSDK;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Input;
using Autodesk.Revit.Attributes;
using static WPFApplication.Property_Copy.WPF_Main_Property_Copy;
using CommunityToolkit.Mvvm.DependencyInjection;
using System.ComponentModel;
using System.Xml.Linq;
using Autodesk.Revit.ApplicationServices;
using Application = Autodesk.Revit.ApplicationServices.Application;
using masshtab;
using WPFApplication.newMaterial_Select_Element_Application;



namespace WPFApplication.newProperty_Copy
{
    /// <summary>
    /// Логика взаимодействия для WPF_Main_Property_Copy.xaml
    /// </summary>
    public partial class newWPF_Main_Property_Copy : Window
    {
        ExternalCommandData commandDat = null;
        public newWPF_Main_Property_Copy(ExternalCommandData commandData)
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

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SSDK;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;



namespace WPFApplication.Property_Copy
{
    /// <summary>
    /// Логика взаимодействия для WPF_Main_Property_Copy.xaml
    /// </summary>
    public partial class WPF_Main_Property_Copy : Window
    {

        public WPF_Main_Property_Copy(ExternalCommandData commandData)
        {
            InitializeComponent();
            Document_Property_Copy.Initialize(commandData);
            Version.Text = SSDK_Data.plugin_Version;
        }

        

       
        private void close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Label_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();
        private void Start_The_Floor_Is_Numeric(object sender, RoutedEventArgs e)
        {

        }

        private void Select_Element(object sender, RoutedEventArgs e)
        {
            Pick_Element pick_Element = new Pick_Element();
            Element element = pick_Element.Pick_Element_Donor() ;
            Select_Element_Donor.Text = Document_Property_Copy.Document.GetElement(element.GetTypeId()).Name.ToString();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TreeView_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void TreeView_PreviewKeyUp(object sender, KeyEventArgs e)
        {

        }

        private void Tree_Parameter_On_Select_Element_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }
    }
}

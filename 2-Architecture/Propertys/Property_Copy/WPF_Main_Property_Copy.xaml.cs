﻿using Autodesk.Revit.DB;
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



namespace WPFApplication.Property_Copy
{
    /// <summary>
    /// Логика взаимодействия для WPF_Main_Property_Copy.xaml
    /// </summary>
    public partial class WPF_Main_Property_Copy : Window
    {
        ExternalCommandData commandDat = null;
        public WPF_Main_Property_Copy(ExternalCommandData commandData)
        {
            
            InitializeComponent();
            commandDat = commandData;
            Version.Text = SSDK_Data.plugin_Version;
            Data_Class_Property_Copy.ParameterCategories = new ObservableCollection<ParameterCategory>();
            DataContext = this;
            //UIApplication uiApp = commandData.Application;
            //Application application = uiApp.Application;
            //Data_Class_Property_Copy.all_Document = application.Documents.Cast<Document>().ToList();
            //foreach (Document document in Data_Class_Property_Copy.all_Document)
            //{
            //    if (!document.IsFamilyDocument && !document.PathName.EndsWith(".rte", StringComparison.OrdinalIgnoreCase))
            //    {
            //        doc_Donor.Items.Add(document.Title);
            //        doc_Target.Items.Add(document.Title);
            //        doc_Donor.SelectedItem = Document_Property_Copy_Donor.Document.Title;
            //        doc_Target.SelectedItem = Document_Property_Copy_Target.Document.Title;
            //    }
            //}
        }
        private void close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Label_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();
        private void Select_Target(object sender, RoutedEventArgs e)
        {
            Pick_Element pick_Element = new Pick_Element();
            pick_Element.Pick_Element_Target(commandDat);
            scroll_Viewer_Work_Set_Ignore.Content = Data_Class_Property_Copy.elements_Target;
        }

        private void Select_Element(object sender, RoutedEventArgs e)
        {

            Pick_Element pick_Element = new Pick_Element();
            Element element = pick_Element.Pick_Element_Donor(commandDat) ;
            Data_Class_Property_Copy.element_Donor = element as FamilyInstance;
            Select_Element_Donor.Text = Document_Property_Copy_Donor.Document.GetElement(element.GetTypeId()).Name.ToString();
            LoadElementParameters loadElementParameters = new LoadElementParameters(element);
            Tree_Parameter_On_Select_Element.ItemsSource = Data_Class_Property_Copy.ParameterCategories;
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

        private void Start_Cod(object sender, RoutedEventArgs e)
        {
            //IList<Parameter_Identification> parameter_Identifications = new List<Parameter_Identification>();
            //LoadParameters loadParameters = new LoadParameters(Data_Class_Property_Copy.element_Donor, Tree_Parameter_On_Select_Element, parameter_Identifications);
            //IList<Parameter_Identification> parameter_Identifications = new List<Parameter_Identification>();
            LoadParameters loadParameters = new LoadParameters();
            Data_Class_Property_Copy.parameters = loadParameters.LoadParameters_Position(Data_Class_Property_Copy.element_Donor, Tree_Parameter_On_Select_Element);
            Close();
        }

        private void Start_The_Floor_Is_Numeric(object sender, RoutedEventArgs e)
        {

        }

        //private void doc_Donor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    foreach (Document document in Data_Class_Property_Copy.all_Document)
        //    {
        //        if (doc_Donor.SelectedItem.ToString() == document.Title)
        //            Document_Property_Copy_Donor.Document = document;   
        //    }
        //}

        //private void doc_Target_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    foreach (Document document in Data_Class_Property_Copy.all_Document)
        //    {
        //        if (doc_Target.SelectedItem.ToString() == document.Title)
        //            Document_Property_Copy_Target.Document = document;
        //    }
        //}
    }
   
}

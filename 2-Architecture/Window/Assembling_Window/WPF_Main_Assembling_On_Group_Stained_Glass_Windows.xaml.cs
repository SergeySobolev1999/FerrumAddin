using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SSDK;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFApplication.Assembling_Window
{
    public partial class WPF_Main_Assembling_Window : Window
    {
        bool preset_False = false;
        public WPF_Main_Assembling_Window()
        {
            InitializeComponent();
            Version.Text = SSDK_Data.plugin_Version;
            Filtered_Assembling_Window filtered_Assembling_Window = new Filtered_Assembling_Window();
            filtered_Assembling_Window.Assembly_Delete();
            filtered_Assembling_Window.Filtered_Position_Assembling_On_Group_Stained_Glass_Windows(preset_False);
            myDataGrid.ItemsSource = Data_Assembling_Window.DataItems;
        }
        private void Add_Collection_Group(object sender, RoutedEventArgs e)
        {
            try
            {

                Data_Assembling_Window.filtered_Group.Clear();
                All_Assembly_Preview_Name_Checked();
                AllPositionElementsInModelAssemblyOrientation();
                Data_Assembling_Window.grup_Filtered_Collection.Clear();
                if (myDataGrid.Items.Count > 0)
                {
                    for (int i = 0; i < myDataGrid.Items.Count; i++)
                    {
                        var row = myDataGrid.Items[i] as DataItem;
                        bool? cellContent = row.Update;
                        int? cellContent_Num = Int32.Parse(row.ID_Group);
                        if (cellContent ?? false)
                        {
                            Data_Assembling_Window.grup_Filtered_Collection.Add(new ElementId((int)cellContent_Num));
                        }
                    }
                    Data_Assembling_Window.grup_Filtered_Collection = Data_Assembling_Window.grup_Filtered_Collection;
                    Position_Create_Assembling_On_Group_Stained_Glass_Windows position_Create_Assembling_On_Group_Stained_Glass_Windows = new Position_Create_Assembling_On_Group_Stained_Glass_Windows();
                    position_Create_Assembling_On_Group_Stained_Glass_Windows.MemberIds_On_Group_Stained_Glass_Windows();
                    RenamedAssemblyExample renamedAssemblyExample = new RenamedAssemblyExample();
                    using (Transaction trans = new Transaction(Revit_Document_Assembling_Window.Document, "Переименование сборок"))
                    {
                        trans.Start();
                       
                        foreach (ElementId group_ElementId in Data_Assembling_Window.grup_Filtered_Collection)
                        {
                            Element element_Group = Revit_Document_Assembling_Window.Document.GetElement(group_ElementId);

                            renamedAssemblyExample.RenamedAssembly(element_Group);
                        }
                        trans.Commit();
                    }
                    position_Create_Assembling_On_Group_Stained_Glass_Windows.Position_Create_Assembling_View_On_Group_Stained_Glass_Windows();
                    if (Data_Assembling_Window.number_Assembly_Elements > 0)
                    {
                        S_Mistake_String s_Mistake_String = new S_Mistake_String("Запись завершена. Успешно обработаных элементов: " + Data_Assembling_Window.number_Assembly_Elements.ToString());
                        s_Mistake_String.ShowDialog();
                    }
                }
                Filtered_Assembling_Window filtered_Assembling_On_Group_Stained_Glass_Windows = new Filtered_Assembling_Window();
                bool position_False = false;
                filtered_Assembling_On_Group_Stained_Glass_Windows.Filtered_Position_Assembling_On_Group_Stained_Glass_Windows(position_False);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        private void Label_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();
        private void close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Select_All(object sender, RoutedEventArgs e)
        {
            try
            {
                bool preset_All_True = true;
                Filtered_Assembling_Window filtered_Assembling_On_Group_Stained_Glass_Windows = new Filtered_Assembling_Window();
                filtered_Assembling_On_Group_Stained_Glass_Windows.Assembly_Delete();
                filtered_Assembling_On_Group_Stained_Glass_Windows.Filtered_Position_Assembling_On_Group_Stained_Glass_Windows(preset_All_True);
                myDataGrid.ItemsSource = Data_Assembling_Window.DataItems;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        private void Cancel_Everything(object sender, RoutedEventArgs e)
        {
            try
            {
                bool preset_All_False = false;
                Filtered_Assembling_Window filtered_Assembling_On_Group_Stained_Glass_Windows = new Filtered_Assembling_Window();
                filtered_Assembling_On_Group_Stained_Glass_Windows.Assembly_Delete();
                filtered_Assembling_On_Group_Stained_Glass_Windows.Filtered_Position_Assembling_On_Group_Stained_Glass_Windows(preset_All_False);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        public void AllPositionElementsInModelAssemblyOrientation()
        {
            Data_Assembling_Window.AssemblyDetailViewOrientation.Clear();
            if ((bool)HorizontalDetail.IsChecked)
            {
                Data_Assembling_Window.AssemblyDetailViewOrientation.Add(AssemblyDetailViewOrientation.HorizontalDetail);
            }
            if ((bool)ElevationLeft.IsChecked)
            {
                Data_Assembling_Window.AssemblyDetailViewOrientation.Add(AssemblyDetailViewOrientation.ElevationLeft);
            }
            if ((bool)ElevationRight.IsChecked)
            {
                Data_Assembling_Window.AssemblyDetailViewOrientation.Add(AssemblyDetailViewOrientation.ElevationRight);
            }
            if ((bool)ElevationFront.IsChecked)
            {
                Data_Assembling_Window.AssemblyDetailViewOrientation.Add(AssemblyDetailViewOrientation.ElevationFront);
            }
            if ((bool)ElevationBack.IsChecked)
            {
                Data_Assembling_Window.AssemblyDetailViewOrientation.Add(AssemblyDetailViewOrientation.ElevationBack);
            }
        }

        private void All_View_Checked(object sender, RoutedEventArgs e)
        {
            bool isChecked = false;
            if (All_View.IsChecked == true)
            {
                isChecked = true;
            }
            HorizontalDetail.IsChecked = isChecked;
            ElevationLeft.IsChecked = isChecked;
            ElevationRight.IsChecked = isChecked;
            ElevationFront.IsChecked = isChecked;
            ElevationBack.IsChecked = isChecked;
        }
        private void All_Assembly_Preview_Name_Checked()
        {
            try
            {
                using (Transaction trans = new Transaction(Revit_Document_Assembling_Window.Document, "Начальное переименование сборок"))
                {
                    trans.Start();
                   
                    FilteredElementCollector window = new FilteredElementCollector(Revit_Document_Assembling_Window.Document);
                    List<Element> all_Elements_Assembly = (List<Element>)window.OfCategory(BuiltInCategory.OST_Assemblies).WhereElementIsNotElementType().ToElements();
                    double position_Iteration = 1000;
                    foreach (AssemblyInstance element in all_Elements_Assembly)
                    {
                        Element element_Type = Revit_Document_Assembling_Window.Document.GetElement(element.GetTypeId());
                        double parameter_Value = element_Type.get_Parameter(Data_Assembling_Window.guid_Group).AsDouble() * 304.8;
                        if (209<= parameter_Value && parameter_Value < 210.999)
                        {
                            element_Type.Name = position_Iteration.ToString();
                            position_Iteration++;
                        }
                    }
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
    }
}

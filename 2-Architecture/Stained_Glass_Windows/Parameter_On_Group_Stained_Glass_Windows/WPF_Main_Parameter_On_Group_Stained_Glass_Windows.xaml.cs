﻿using Autodesk.Revit.DB;
using SSDK;
using System;
using System.Collections.Generic;
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

namespace WPFApplication.Parameter_On_Group_Stained_Glass_Windows
{
    /// <summary>
    /// Логика взаимодействия для WPF_Main_Parameter_On_Group_Stained_Glass_Windows.xaml
    /// </summary>
    public partial class WPF_Main_Parameter_On_Group_Stained_Glass_Windows : Window
    {
        public WPF_Main_Parameter_On_Group_Stained_Glass_Windows()
        {
            
            InitializeComponent();
            Version.Text = SSDK_Data.plugin_Version;
        }

        private void Start_The_Floor_Is_Numeric(object sender, RoutedEventArgs e)
        {
            try
            {
                Data_Parameter_On_Group_Stained_Glass_Windows.error_Suppressio = false;
                if ((bool)Error_Suppressio.IsChecked)
                {
                    Data_Parameter_On_Group_Stained_Glass_Windows.error_Suppressio = true;
                }
                Data_Parameter_On_Group_Stained_Glass_Windows.number_Elements = 0;
                Data_Parameter_On_Group_Stained_Glass_Windows.filtered_Group.Clear();
                Data_Parameter_On_Group_Stained_Glass_Windows.list_Group.Clear();
                Filtered_Mark_On_Group_Stained_Glass_Windows filtered_Mark_On_Group_Stained_Glass_Windows = new Filtered_Mark_On_Group_Stained_Glass_Windows();
                Collecting_Group_Stained_Glass_Windows collecting_Group_Stained_Glass_Windows = new Collecting_Group_Stained_Glass_Windows();
                Sort_On_Parameter collecting_Windows = new Sort_On_Parameter();
                if (Data_Parameter_On_Group_Stained_Glass_Windows.iteration_Recaive_Value_In_Parameter == true)
                {

                    S_Mistake_String s_Mistake_String_Warning = new S_Mistake_String(Data_Parameter_On_Group_Stained_Glass_Windows.iteration_Recaive_Value_In_Parameter_Watringn);
                    s_Mistake_String_Warning.ShowDialog();
                }
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Запись завершена. Успешно обработаных элементов: " + Data_Parameter_On_Group_Stained_Glass_Windows.number_Elements.ToString());
                Close();
                s_Mistake_String.ShowDialog();

            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                Close();
                s_Mistake_String.ShowDialog();
            }
        }
        private void close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Label_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();
    }
}

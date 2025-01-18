using Autodesk.Revit.DB;
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

namespace WPFApplication.Mark_Window
{
    /// <summary>
    /// Логика взаимодействия для WPF_Main_Parameter_On_Group_Stained_Glass_Windows.xaml
    /// </summary>
    public partial class WPF_Main_Mark_Window : Window
    {
        public WPF_Main_Mark_Window()
        {
            
            InitializeComponent();
        }

        private void Start_The_Floor_Is_Numeric(object sender, RoutedEventArgs e)
        {
            try
            {
                Data_Mark_Window.error_Suppressio = false;
                if ((bool)Error_Suppressio4.IsChecked)
                {
                    Data_Mark_Window.error_Suppressio = true;
                }
                Data_Mark_Window.filtered_Group.Clear();
                Data_Mark_Window.list_Group.Clear();
                Data_Mark_Window.number_Elements = 0;
                Data_Mark_Window.number_Elements_Transom = 0;
                Filtered_Mark_Window filtered_Mark_Window = new Filtered_Mark_Window();
                Collecting_Window collecting_Group_Stained_Glass_Windows = new Collecting_Window();
                Sort_On_Mark_Window sort_On_Mark_Window = new Sort_On_Mark_Window();
                if (Data_Mark_Window.iteration_Recaive_Value_In_Parameter == true)
                {

                    S_Mistake_String s_Mistake_String_Warning = new S_Mistake_String(Data_Mark_Window.iteration_Recaive_Value_In_Parameter_Watringn);
                    
                    s_Mistake_String_Warning.ShowDialog();
                }
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Запись завершена. Успешно обработаных элементов: " + (Data_Mark_Window.number_Elements + Data_Mark_Window.number_Elements_Transom).ToString());
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

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

namespace RevitAPI_Basic_Course.Creating_Specifications
{
    /// <summary>
    /// Логика взаимодействия для Set_WPF_Creating_Specifications.xaml
    /// </summary>
    public partial class Set_WPF_Creating_Specifications : Window
    {
        public Set_WPF_Creating_Specifications()
        {
            InitializeComponent();
        }

        private void closeWPF_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (selectef_Save_Position.Text.ToString() != "")
            {
                Data_Creating_Specifications.name_Save_Element_Positon = selectef_Save_Position.Text.ToString();
                Close();
            }
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();
    }
}

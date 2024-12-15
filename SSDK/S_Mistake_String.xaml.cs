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

namespace SSDK
{
    /// <summary>
    /// Логика взаимодействия для S_Mistake_String.xaml
    /// </summary>
    public partial class S_Mistake_String : Window
    {
        public S_Mistake_String(string name)
        {
            
            InitializeComponent();
            Version.Text = SSDK_Data.plugin_Version;
            string_Mistake_Position(name);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();
        private void string_Mistake_Position(string name)
        {
            inputTextBox.Text = name;
        }
    }
}

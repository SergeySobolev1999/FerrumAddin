using System.Xaml;
using System.Windows;

namespace masshtab
{
    /// <summary>
    /// Логика взаимодействия для iWindow.xaml
    /// </summary>
    public partial class iWindow : Window
    {
        public iWindow(string txt)
        {
            InitializeComponent();
            text1.Text += txt;
            this.SizeToContent = SizeToContent.Height;
        }

        private void escButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close(); // закрытие окна
        }


    }
}

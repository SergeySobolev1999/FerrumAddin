using System.Xaml;
using System.Windows;

namespace masshtab
{
    /// <summary>
    /// Логика взаимодействия для wswpf.xaml
    /// </summary>
    public partial class wswpf : Window
    {
        public wswpf(wsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close(); // закрытие окна
        }

        private void escButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close(); // закрытие окна
        }


    }
}

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

namespace FerrumAddin.ModelStatistics
{
    /// <summary>
    /// Логика взаимодействия для WpfMainVeiw.xaml
    /// </summary>
    public partial class WpfMainVeiw : Window
    {
        public WpfMainVeiw(StatisticsViewModel viewModel)
        {
            InitializeComponent();
            Version.Text = SSDK_Data.plugin_Version;
            this.DataContext = viewModel;
        }
        private void Label_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();
        private void close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

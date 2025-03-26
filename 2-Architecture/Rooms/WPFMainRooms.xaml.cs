using Autodesk.Revit.UI;
using SSDK;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace WPFApplication.Rooms
{
    /// <summary>
    /// Логика взаимодействия для WPFMainRooms.xaml
    /// </summary>
    public partial class WPFMainRooms : UserControl
    {
        public WPFMainRooms()
        {
            InitializeComponent();
            RoomListBox.SelectionChanged += RoomListBox_SelectionChanged;
            Version.Text = SSDK_Data.plugin_Version;
        }
        private void RoomListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is MyDockablePaneViewModel vm)
            {
                vm.SelectedRooms.Clear();
                foreach (var item in RoomListBox.SelectedItems.OfType<RoomsPosition>())
                    vm.SelectedRooms.Add(item);
            }
        }
        //private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (DataContext is MyDockablePaneViewModel vm)
        //    {
        //        vm.SelectedRooms.Clear();
        //        foreach (var item in ((ListBox)sender).SelectedItems)
        //        {
        //            if (item is RoomsPosition room)
        //            {
        //                vm.SelectedRooms.Add(room);
        //            }
        //        }
        //    }
        //}
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MyDockablePaneViewModel;
            var cb = sender as CheckBox;
            var item = cb?.Content?.ToString();
            if (!string.IsNullOrEmpty(item) && vm.SelectedFunctionalPurposes.Contains(item))
            {
                vm.SelectedFunctionalPurposes.Remove(item);
                vm.FilteredRooms.Refresh();
            }
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MyDockablePaneViewModel;
            var cb = sender as CheckBox;
            var item = cb?.Content?.ToString();
            if (!string.IsNullOrEmpty(item) && !vm.SelectedFunctionalPurposes.Contains(item))
            {
                vm.SelectedFunctionalPurposes.Add(item);
                vm.FilteredRooms.Refresh();
            }
        }
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FilterPopup.IsOpen = true;
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
        }
        private void WPFMainRooms_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MyDockablePaneViewModel;
            RoomListBox.SelectionChanged += (s, args) =>
            {
                vm.SelectedRooms.Clear();
                foreach (var item in RoomListBox.SelectedItems.OfType<RoomsPosition>())
                {
                    vm.SelectedRooms.Add(item);
                }
            };
        }
        public class BooleanToVisibilityConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is bool isVisible && isVisible)
                    return System.Windows.Visibility.Visible;
                else
                    return System.Windows.Visibility.Collapsed;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class BooleanToBrushConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is bool isSelected && isSelected)
                {
                    return Brushes.LightBlue;
                }
                return Brushes.White;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }


}

using System;
using System.Globalization;
using System.Windows.Data;
using System.Collections.ObjectModel;

namespace WPFApplication.Rooms.Converters
{
    public class ListContainsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var selectedList = value as ObservableCollection<string>;
            var item = parameter as string ?? value?.ToString();
            return selectedList?.Contains(item) ?? false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }
}
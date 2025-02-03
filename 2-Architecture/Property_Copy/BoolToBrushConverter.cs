//using System;
//using System.Globalization;
//using System.Windows.Data;
//using System.Windows.Media;

//namespace WPFApplication.Property_Copy
//{
//    public class BoolToBrushConverter : IValueConverter
//    {
//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            if (value is bool isSelected && isSelected)
//            {
//                return new SolidColorBrush(Colors.LightBlue);
//            }
//            return new SolidColorBrush(Colors.Transparent);
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}

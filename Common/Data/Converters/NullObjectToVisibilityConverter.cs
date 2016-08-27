using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class NullObjectToVisibilityConverter : IValueConverter
    {
        public enum Parameter
        {
            Normal,
            Inverted
        }
        private Visibility GetOpposite(Visibility Visibility)
        {
            return Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Parameter Parameter = parameter == null ? Parameter.Normal : (Parameter)Enum.Parse(typeof(Parameter), (string)parameter);
            Visibility Visibility = value == null ? Visibility.Collapsed : Visibility.Visible;
            return Parameter == Parameter.Normal ? Visibility : this.GetOpposite(Visibility);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

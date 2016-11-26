using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class NullToVisibilityConverter : IValueConverter
    {
        public enum Parameter
        {
            Normal,
            Inverted
        }
        Visibility Evaluate(Visibility Visibility, Parameter Parameter)
        {
            return Parameter == Parameter.Normal ? Visibility : (Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible);
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Parameter Parameter = parameter == null ? Parameter.Normal : (Parameter)Enum.Parse(typeof(Parameter), (string)parameter);
            return this.Evaluate(value == null ? Visibility.Collapsed : Visibility.Visible, Parameter);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(string), typeof(Visibility))]
    public class EmptyStringToVisibilityConverter : IValueConverter
    {
        enum Parameters
        {
            Normal, Inverted
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Parameters Parameter = parameter == null ? Parameters.Normal : (Parameters)Enum.Parse(typeof(Parameters), (string)parameter);
            return (value as string) == null || (value as string).Length == 0 ? (Parameter == Parameters.Normal ? Visibility.Collapsed : Visibility.Visible) : (Parameter == Parameters.Normal ? Visibility.Visible : Visibility.Collapsed);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

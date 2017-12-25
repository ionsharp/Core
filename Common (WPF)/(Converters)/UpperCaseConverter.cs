using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(object), typeof(string))]
    public class UpperCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString().ToUpper();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

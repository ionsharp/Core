using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class ValueEqualsParameterToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value == parameter).ToVisibility();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter;
        }
    }
}

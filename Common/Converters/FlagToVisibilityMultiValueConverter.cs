using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Imagin.Common.Extensions;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(object[]), typeof(Visibility))]
    public class FlagToVisibilityMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length == 2)
                return ((Enum)values[0]).HasFlag((Enum)values[1]).ToVisibility();
            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

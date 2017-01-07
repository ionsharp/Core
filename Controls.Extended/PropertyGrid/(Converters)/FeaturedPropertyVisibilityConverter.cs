using Imagin.Common.Extensions;
using System;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Controls.Extended.Converters
{
    [ValueConversion(typeof(object[]), typeof(Visibility))]
    public class FeaturedPropertyVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values != null && values.Length == 2 && values[1] != null)
                return ((bool)values[0]).ToVisibility();
            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

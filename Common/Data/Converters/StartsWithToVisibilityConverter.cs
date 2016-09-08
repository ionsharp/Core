using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(object[]), typeof(Visibility))]
    public class StartsWithToVisibilityMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length == 2)
            {
                string ToEvaluate = values[0].ToString();
                string Search = values[1].ToString();
                if (!string.IsNullOrEmpty(ToEvaluate) && !string.IsNullOrEmpty(Search))
                {
                    if (!ToEvaluate.ToLower().StartsWith(Search))
                        return Visibility.Collapsed;
                }
            }
            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

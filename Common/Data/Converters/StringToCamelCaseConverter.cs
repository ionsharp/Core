using Imagin.Common.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class StringToCamelCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string)) return value.ToString().SplitCamelCase();
            string Value = (string)value;
            return Value == null || Value == string.Empty ? string.Empty : Value.SplitCamelCase();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

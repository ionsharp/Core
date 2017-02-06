using System;
using System.Globalization;
using System.Windows.Data;
using Imagin.Common.Extensions;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(Enum), typeof(string))]
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? string.Empty : value.ToString().SplitCamelCase();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

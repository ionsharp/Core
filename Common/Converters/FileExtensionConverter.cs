using Imagin.Common.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class FileExtensionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;
            var Value = (string)value;
            if (Value.IsNullOrEmpty()) return string.Empty;
            return Value.GetExtension();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

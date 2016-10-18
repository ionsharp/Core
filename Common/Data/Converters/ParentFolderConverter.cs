using Imagin.Common.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class ParentFolderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;
            var Value = (string)value;
            if (Value.IsNullOrEmpty()) return string.Empty;
            if (Value.IsValidUrl(Uri.UriSchemeFile))
                return Value.GetDirectoryName();
            else if (Value.IsValidUrl(Uri.UriSchemeFtp))
                return Value.GetFtpDirectoryName();
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

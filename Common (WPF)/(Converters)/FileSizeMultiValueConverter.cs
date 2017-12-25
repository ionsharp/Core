using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(object[]), typeof(string))]
    public class FileSizeMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length == 2 && values[0] is long && values[1] is FileSizeFormat)
            {
                var Value = (long)values[0];
                var FileSizeFormat = (FileSizeFormat)values[1];
                return Value.ToFileSize(FileSizeFormat);
            }
            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

using Imagin.Common.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(long), typeof(string))]
    public class FileSizeConverter : IValueConverter
    {
        FileSizeFormat GetFileSizeFormat(object Parameter)
        {
            return Parameter == null ? FileSizeFormat.BinaryUsingSI : (Parameter is FileSizeFormat ? (FileSizeFormat)Parameter : (FileSizeFormat)Enum.Parse(typeof(FileSizeFormat), Parameter.ToString()));
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && value.Is<long>() ? value.As<long>().ToFileSize(GetFileSizeFormat(parameter)) : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

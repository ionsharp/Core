using Imagin.Common.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(long), typeof(string))]
    public class FileSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            else if (value.Is<string>())
            {
                long Value = 0L;
                if (System.IO.File.Exists(value.ToString()))
                    Value = new System.IO.FileInfo(value.ToString()).Length;
                else Value = value.ToString().ToLong();
                return Value.ToFileSize();
            }
            else if (value.Is<long>())
                return value.As<long>().ToFileSize();
            else return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

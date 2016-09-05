using Imagin.Common.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(double), typeof(string))]
    public class FileSpeedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            double Value = 0.0;
            if (value.Is<string>())
                Value = value.ToString().ToDouble();
            else if (value.Is<double>())
                Value = value.As<double>();
            return Value == 0 ? string.Empty : string.Concat(Value.ToFileSize(), "/s");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

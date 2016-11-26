using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(DateTime), typeof(string))]
    public class DateTimeStringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value is DateTime || value is DateTime?) && parameter != null)
            {
                var Date = value is DateTime? ? ((DateTime?)value).Value : (DateTime)value;
                return Date.ToString(parameter.ToString());
            }
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

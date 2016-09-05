using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(DateTime), typeof(int))]
    public class DateTimeToDayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 0;
            DateTime Date = default(DateTime);
            if (value is Nullable<DateTime>)
                Date = ((Nullable<DateTime>)value).Value;
            else if (value is DateTime) Date = (DateTime)value;
            else return 0;
            return Date.Day;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

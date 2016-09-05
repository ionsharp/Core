using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(DateTime), typeof(bool))]
    public class DateTimeIsTodayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;

            DateTime Date = default(DateTime);
            if (value is Nullable<DateTime>)
                Date = ((Nullable<DateTime>)value).Value;
            else if (value is DateTime) Date = (DateTime)value;
            else return false;
            DateTime Today = DateTime.Today;

            return Date.Month == Today.Month && Date.Day == Today.Day && Date.Year == Today.Year;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

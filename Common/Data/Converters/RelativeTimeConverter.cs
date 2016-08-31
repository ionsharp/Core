using Imagin.Common.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(DateTime), typeof(string))]
    public class RelativeTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "Never";
            DateTime Date = default(DateTime);
            if (value is Nullable<DateTime>)
                Date = ((Nullable<DateTime>)value).Value;
            else if (value is DateTime)
                Date = (DateTime)value;
            else
                return "Never";
            return Date.ToRelativeTime();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

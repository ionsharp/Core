using System;
using Windows.UI.Xaml.Data;

namespace Imagin.Common.Data
{
    public class DateTimeToDateTimeOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return new DateTimeOffset((DateTime)value);
            }
            catch (Exception)
            {
                return DateTimeOffset.MinValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return ((DateTimeOffset)value).DateTime;
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }
    }
}

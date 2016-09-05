using Imagin.Common.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(int), typeof(string))]
    public class SecondsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            if (value.Is<int>())
                return TimeSpan.FromSeconds(value.As<int>()).ToShortTime();
            else if (value.Is<string>())
                return TimeSpan.FromSeconds(value.ToString().ToInt()).ToShortTime();
            else return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

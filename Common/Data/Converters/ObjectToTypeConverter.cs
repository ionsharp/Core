using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(object), typeof(Type))]
    public class ObjectToTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            if (parameter != null && parameter.ToString() == "asdf")
            {
                Console.WriteLine("\n\n" + value.GetType().ToString());
            }
            return value.GetType();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

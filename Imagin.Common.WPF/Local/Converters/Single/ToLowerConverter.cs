using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Local.Converters
{
    public class ToLowerConverter : TypeValueConverterBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return value.ToString().ToLower();

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        } 
    }
}
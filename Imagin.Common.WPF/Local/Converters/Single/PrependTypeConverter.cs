using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Local.Converters
{
    /// <summary>
    /// PrependTypeConverter allows to prepend the type of the value as string with the default _ separator. To change the default separator just us the converterparamater
    /// </summary>
    public class PrependTypeConverter : TypeValueConverterBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var separator = "_";
                if (parameter != null && parameter.GetType() == typeof(string))
                    separator = parameter.ToString();
                return value.GetType().Name + separator + value.ToString();
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        } 
    }
}
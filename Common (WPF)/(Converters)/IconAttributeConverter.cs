using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(Enum), typeof(string))]
    public class IconAttributeConverter : IValueConverter
    {
        public object Convert(object Value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Value != null)
            {
                var Attribute = Value.GetAttribute<IconAttribute>(!Value.GetType().IsEnum && parameter != null ? parameter.ToString() : string.Empty);
                if (Attribute != null)
                    return Attribute.Uri;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

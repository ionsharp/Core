using Imagin.Common.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class SubstringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!value.IsNull() && (value.Is<string>() || value.Is<Enum>()))
            {
                if (parameter != null)
                {
                    var v = value.ToString();
                    int Length = v.Length;
                    int.TryParse(parameter.ToString(), out Length);
                    return v.Substring(0, Length);
                }
                return value.ToString();
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

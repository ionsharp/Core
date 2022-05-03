using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object[]), typeof(string))]
    public class StringMultiConverter : MultiConverter<string>
    {
        public static StringMultiConverter Default { get; private set; } = new StringMultiConverter();
        StringMultiConverter() { }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null)
            {
                var result = string.Empty;
                foreach (var i in values)
                    result = $"{result}{i}";

                return result;
            }
            return null;
        }
    }
}
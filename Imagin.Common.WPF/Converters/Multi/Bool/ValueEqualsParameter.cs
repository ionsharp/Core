using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object[]), typeof(bool))]
    public class ValueEqualsParameterMultiConverter : MultiConverter<bool>
    {
        public static ValueEqualsParameterMultiConverter Default { get; private set; } = new ValueEqualsParameterMultiConverter();
        ValueEqualsParameterMultiConverter() { }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 2)
                return values[0]?.Equals(values[1]);

            return default(bool);
        }
    }
}
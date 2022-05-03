using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object[]), typeof(string))]
    public class PropertyChangedMultiConverter : MultiConverter<string>
    {
        public static PropertyChangedMultiConverter Default { get; private set; } = new PropertyChangedMultiConverter();
        PropertyChangedMultiConverter() { }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length > 1)
                return values[0].ToString();

            return Binding.DoNothing;
        }
    }
}
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object[]), typeof(string))]
    public class StringReplaceMultiConverter : MultiConverter<string>
    {
        public static StringReplaceMultiConverter Default { get; private set; } = new StringReplaceMultiConverter();
        StringReplaceMultiConverter() { }

        public sealed override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 3)
            {
                if (values[0] is object i)
                {
                    if (values[1] is string a)
                    {
                        if (values[2] is string b)
                            return $"{i}"?.Replace(a, b);
                    }
                }
            }
            return Binding.DoNothing;
        }
    }
}
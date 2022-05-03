using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object[]), typeof(string))]
    public class SubstringMultiConverter : MultiConverter<string>
    {
        public static SubstringMultiConverter Default { get; private set; } = new SubstringMultiConverter();
        SubstringMultiConverter() { }

        public sealed override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length >= 2)
            {
                if (values[0] is object a)
                {
                    if (values[1] is int b)
                    {
                        var result = $"{a}";
                        return Try.Invoke(() =>
                        {
                            if (values?.Length == 3)
                            {
                                if (values[2] is int c)
                                {
                                    result = result.Substring(b, c);
                                    return;
                                }
                            }
                            result = result.Substring(b);
                        })
                        ? result : "";
                    }
                }
            }
            return Binding.DoNothing;
        }
    }
}
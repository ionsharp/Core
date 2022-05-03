using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object[]), typeof(string))]
    public class TimeLeftMultiConverter : MultiConverter<string>
    {
        public static TimeLeftMultiConverter Default { get; private set; } = new TimeLeftMultiConverter();
        TimeLeftMultiConverter() { }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length == 3)
            {
                if (values[0] is TimeSpan && values[1] is long && values[2] is long)
                {
                    var bytesRead = (long)values[1];
                    var bytesTotal = (long)values[2];

                    var result = (TimeSpan)values[0];
                    return TimeSpan.FromSeconds(result.Left(bytesRead, bytesTotal).TotalSeconds.Round()).ToString();
                }
            }
            return string.Empty;
        }
    }
}
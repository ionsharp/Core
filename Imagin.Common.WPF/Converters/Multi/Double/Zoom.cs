using System;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object[]), typeof(double))]
    public class ZoomMultiConverter : MultiConverter<double>
    {
        public static ZoomMultiConverter Default { get; private set; } = new ZoomMultiConverter();
        ZoomMultiConverter() { }

        public override object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] is double)
            {
                var value = (double)values[0];
                var zoom = (double)values[1];

                return value / zoom;
            }
            return default(double);
        }
    }
}
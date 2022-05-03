using Imagin.Common.Linq;
using Imagin.Common.Media;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(double), typeof(double))]
    public class GraphicUnitMultiConverter : MultiConverter<double>
    {
        public static GraphicUnitMultiConverter Default { get; private set; } = new();
        GraphicUnitMultiConverter() { }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length >= 4)
            {
                if (values[0] is double value)
                {
                    if (values[1] is float resolution)
                    {
                        if (values[2] is GraphicUnit from)
                        {
                            if (values[3] is GraphicUnit to)
                            {
                                var result = value.Convert(from, to, resolution);
                                if (values.Length >= 5)
                                {
                                    if (values[4] is double scale)
                                        return result * scale;
                                }
                                return result;
                            }
                        }
                    }
                }
            }
            return Binding.DoNothing;
        }
    }
}
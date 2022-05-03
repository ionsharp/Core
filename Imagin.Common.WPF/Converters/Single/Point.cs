using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(double), typeof(Point))]
    public class DoubleToPointConverter : Converter<double, Point>
    {
        public static DoubleToPointConverter Default { get; private set; } = new DoubleToPointConverter();
        DoubleToPointConverter() { }

        protected override ConverterValue<Point> ConvertTo(ConverterData<double> input)
            => new Point(input.Value, input.Value);
    }
}
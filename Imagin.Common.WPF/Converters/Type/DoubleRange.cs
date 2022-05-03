using Imagin.Common.Numbers;

namespace Imagin.Common.Converters
{
    public class DoubleRangeTypeConverter : StringTypeConverter<double>
    {
        protected override int? Length => 2;

        protected override double Convert(string input) => double.Parse(input);

        protected override object Convert(double[] input) => new DoubleRange(input[0], input[1]);
    }
}
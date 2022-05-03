using Imagin.Common.Numbers;

namespace Imagin.Common.Converters
{
    public class AngleRangeTypeConverter : StringTypeConverter<Angle>
    {
        protected override int? Length => 2;

        protected override Angle Convert(string input) => double.Parse(input);

        protected override object Convert(Angle[] input) => new AngleRange(input[0], input[1]);
    }
}
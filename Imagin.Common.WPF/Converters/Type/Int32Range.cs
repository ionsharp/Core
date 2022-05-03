namespace Imagin.Common.Converters
{
    public class Int32RangeTypeConverter : StringTypeConverter<int>
    {
        protected override int? Length => 2;

        protected override int Convert(string input) => int.Parse(input);

        protected override object Convert(int[] input) => new Range<int>(input[0], input[1]);
    }
}
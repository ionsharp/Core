using Imagin.Colour.Primitives;

namespace Imagin.Colour.Conversion
{
#pragma warning disable 1591
    public class YDbDrConverter : ColorConverterBase<YDbDr>
    {
        public YDbDr Convert(RGB input)
        {
            return new YDbDr
            (
                 0.299 * input.R + 0.587 * input.G + 0.114 * input.B,
                -0.450 * input.R - 0.883 * input.G + 1.333 * input.B,
                -1.333 * input.R + 1.116 * input.G + 0.217 * input.B
            );
        }
    }
#pragma warning restore 1591
}
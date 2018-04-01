using Imagin.Colour.Primitives;

namespace Imagin.Colour.Conversion
{
#pragma warning disable 1591
    public class YCoCgConverter : ColorConverterBase<YCoCg>
    {
        public YCoCg Convert(RGB input)
        {
            return new YCoCg
            (
                 0.25 * input.R + 0.5 * input.G + 0.25 * input.B,
                -0.25 * input.R + 0.5 * input.G - 0.25 * input.B,
                 0.5  * input.R - 0.5 * input.B
            );
        }
    }
#pragma warning restore 1591
}
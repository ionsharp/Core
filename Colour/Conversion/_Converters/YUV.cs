using Imagin.Colour.Primitives;
using Imagin.Common.Linq;

namespace Imagin.Colour.Conversion
{
#pragma warning disable 1591
    public class YUVConverter : ColorConverterBase<YUV>
    {
        public YUV Convert(RGB input)
        {
            var y =  0.299 * input.R + 0.587 * input.G + 0.114 * input.B;
            var u = -0.147 * input.R - 0.289 * input.G + 0.436 * input.B;
            var v =  0.615 * input.R - 0.515 * input.G - 0.100 * input.B;
            return new YUV(y, u, v);
        }
    }
#pragma warning restore 1591
}

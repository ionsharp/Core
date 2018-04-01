using Imagin.Colour.Primitives;
using System;

namespace Imagin.Colour.Conversion
{
#pragma warning disable 1591
    public class HWBConverter : ColorConverterBase<HWB>
    {
        public HWB Convert(RGB input)
        {
            double r = input.R, g = input.G, b = input.B;

            var _h = 0; //Get HSL.Hue of input
            var _w = 1 / 255 * Math.Min(r, Math.Min(g, b));
            var _b = 1 - 1 / 255 * Math.Max(r, Math.Max(g, b));

            return new HWB(_h, _w * 100, _b * 100);
        }
    }
#pragma warning restore 1591
}
using Imagin.Colour.Primitives;
using System;

namespace Imagin.Colour.Conversion
{
#pragma warning disable 1591
    public class HSIConverter : ColorConverterBase<HSI>
    {
        public HSI Convert(RGB input)
        {
            var _input = input.Vector * 255;
            var sum = _input.Sum();

            var r = _input[0] / sum;
            var g = _input[1] / sum;
            var b = _input[2] / sum;

            var h = Math.Acos
            (
                (0.5 * ((r - g) + (r - b))) 
                / 
                Math.Sqrt
                (
                    (r - g) * (r - g) + (r - b) * (g - b)
                )
            );

            if (b > g)
                h = 2 * Math.PI - h;

            var s = 1 - 3 * Math.Min(r, Math.Min(g, b));
            var i = sum / 3;

            return new HSI(h * 180 / Math.PI, s * 100, i);
        }
    }
#pragma warning restore 1591
}
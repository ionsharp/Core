using Imagin.Colour.Primitives;
using System;

namespace Imagin.Colour.Conversion
{
#pragma warning disable 1591
    public class HCGConverter : ColorConverterBase<HCG>
    {
        public HCG Convert(RGB input)
        {
            double r = input.R, g = input.G, b = input.B;

            var maximum = Math.Max(Math.Max(r, g), b);
            var minimum = Math.Min(Math.Min(r, g), b);

            var chroma = maximum - minimum;
            double grayscale = 0;
            double hue;

            if (chroma < 1)
                grayscale = minimum / (1.0 - chroma);

            if (chroma > 0)
            {
                if (maximum == r)
                {
                    hue = ((g - b) / chroma) % 6;
                }
                else if (maximum == g)
                {
                    hue = 2 + (b - r) / chroma;
                }
                else hue = 4 + (r - g) / chroma;

                hue /= 6;
                hue = hue % 1;
            }
            else hue = 0;

            return new HCG(hue * 360.0, chroma * 100.0, grayscale * 100.0);
        }
    }
#pragma warning restore 1591
}
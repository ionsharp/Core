using Imagin.Colour.Primitives;
using System;

namespace Imagin.Colour.Conversion
{
#pragma warning disable 1591
    public class HSLConverter : ColorConverterBase<HSL>
    {
        public HSL Convert(RGB input)
        {
            var maximum = Math.Max(Math.Max(input.R, input.G), input.B);
            var minimum = Math.Min(Math.Min(input.R, input.G), input.B);

            var chroma = maximum - minimum;

            double h = 0, s = 0, l = (maximum + minimum) / 2.0;

            if (chroma != 0)
            {
                s
                    = l < 0.5
                    ? chroma / (2.0 * l)
                    : chroma / (2.0 - 2.0 * l);

                if (input.R == maximum)
                {
                    h = (input.G - input.B) / chroma;
                    h 
                        = input.G < input.B
                        ? h + 6.0 
                        : h;
                }
          else if (input.B == maximum)
                {
                    h = 4.0 + ((input.R - input.G) / chroma);
                }
          else if (input.G == maximum)
                    h = 2.0 + ((input.B - input.R) / chroma);

                h *= 60;
            }

            return new HSL(h, s, l);
        }
    }
#pragma warning restore 1591
}
using Imagin.Common.Linq;
using Imagin.Colour.Primitives;
using System;

namespace Imagin.Colour.Conversion
{
#pragma warning disable 1591
    public class TSLConverter : ColorConverterBase<TSL>
    {
        public TSL Convert(RGB input)
        {
            double r = input.R, g = input.G, b = input.B;
            double rgb = r + g + b;

            double r_ = (r / rgb) - 1.0 / 3.0, 
                   g_ = (g / rgb) - 1.0 / 3.0;

            var T = .0;

            if (g_ > 0)
            {
                T = .5 * Math.PI * Math.Atan(r_ / g_) + .25;
            }
            else if (g_ < 0)
            {
                T = .5 * Math.PI * Math.Atan(r_ / g_) + .75;
            }

            var S = Math.Sqrt(9.0 / 5.0 * (r_ * r_ + g_ * g_));

            var L = (r * .299) + (g * .587) + (b * .114);

            return new TSL(T, S, L);
        }
    }
#pragma warning restore 1591
}
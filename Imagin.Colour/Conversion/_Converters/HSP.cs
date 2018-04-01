using Imagin.Colour.Primitives;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Conversion
{
#pragma warning disable 1591
    public class HSPConverter : ColorConverterBase<HSP>
    {
        public HSP Convert(RGB input)
        {
            const double Pr = 0.299;
            const double Pg = 0.587;
            const double Pb = 0.114;

            var _input = input.Vector * 255;
            double r = _input[0], g = _input[1], b = _input[2];
            double h = 0, s = 0, p = 0;

            p = Math.Sqrt(r * r * Pr + g * g * Pg + b * b * Pb);

            if (r == g && r == b)
            {
                h = 0.0;
                s = 0.0;
            }
            else
            {
                //R is largest
                if (r >= g && r >= b)
                {
                    if (b >= g)
                    {
                        h = 6.0 / 6.0 - 1.0 / 6.0 * (b - g) / (r - g);
                        s = 1.0 - g / r;
                    }
                    else
                    {
                        h = 0.0 / 6.0 + 1.0 / 6.0 * (g - b) / (r - b);
                        s = 1.0 - b / r;
                    }
                }

                //G is largest
                if (g >= r && g >= b)
                {
                    if (r >= b)
                    {
                        h = 2.0 / 6.0 - 1.0 / 6.0 * (r - b) / (g - b);
                        s = 1 - b / g;
                    }
                    else
                    {
                        h = 2.0 / 6.0 + 1.0 / 6.0 * (b - r) / (g - r);
                        s = 1.0 - r / g;
                    }
                }

                //B is largest
                if (b >= r && b >= g)
                {
                    if (g >= r)
                    {
                        h = 4.0 / 6.0 - 1.0 / 6.0 * (g - r) / (b - r);
                        s = 1.0 - r / b;
                    }
                    else
                    {
                        h = 4.0 / 6.0 + 1.0 / 6.0 * (r - g) / (b - g);
                        s = 1.0 - g / b;
                    }
                }
            }
            return new HSP((h * 360.0).Round(), s * 100.0, p.Round());
        }
    }
#pragma warning restore 1591
}
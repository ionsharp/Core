using Imagin.Colour.Primitives;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Conversion
{
#pragma warning disable 1591
    public class HSBConverter : ColorConverterBase<HSB>
    {
        public HSB Convert(RGB input)
        {
            double r = input.R, g = input.G, b = input.B;

            var minimum = Math.Min(input.R, Math.Min(input.G, input.B));
            var maximum = Math.Max(input.R, Math.Max(input.G, input.B));

            var chroma = maximum - minimum;

            var _h = 0.0;
            var _s = 0.0;
            var _b = maximum;

            if (chroma == 0)
            {
                _h = 0;
                _s = 0;
            }
            else
            {
                _s = chroma / maximum;

                if (input.R == maximum)
                {
                    _h = (input.G - input.B) / chroma;
                    _h = input.G < input.B ? _h + 6 : _h;
                }
                else if (input.G == maximum)
                {
                    _h = 2.0 + ((input.B - input.R) / chroma);
                }
                else if (input.B == maximum)
                    _h = 4.0 + ((input.R - input.G) / chroma);

                _h *= 60;
            }

            return new HSB(_h, _s.Shift(2), _b.Shift(2));
        }
    }
#pragma warning restore 1591
}
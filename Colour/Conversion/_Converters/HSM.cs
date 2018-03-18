using Imagin.Colour.Primitives;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Conversion
{
#pragma warning disable 1591
    public class HSMConverter : ColorConverterBase<HSM>
    {
        public HSM Convert(RGB input)
        {
            var m = (4.0 * input.R + 2.0 * input.G + input.B) / 7.0;

            var a = 3.0 * (input.R - m) - 4.0 * (input.G - m) - 4.0 * (input.B - m);
            var b = 41.0.SquareRoot();
            var c = a / b;

            var d = (input.R - m).Power(2) + (input.G - m).Power(2) + (input.B - m).Power(2);
            var e = d.SquareRoot();

            var f = c / e;

            var theta = Math.Acos(f);
            var w = .0;

            if (input.B <= input.G)
            {
                w = theta;
            }
            else if (input.B > input.G)
            {
                w = 2.0 * Math.PI - theta;
            }

            var hue = w / (2.0 * Math.PI);
            var saturation = .0;
            var mixture = m;

            double q = .0, r = .0, s = .0;

            if (0 <= m && m <= 1.0 / 7.0)
            {
                q = 0.0;
                r = 0.0;
                s = 7.0;
            }
            else if(1.0 / 7.0 <= m && m <= 3.0 / 7.0)
            {
                q = .0;
                r = ((7.0 * m) - 1.0) / 2.0;
                s = 1.0;
            }
            else if(3.0 / 7.0 <= m && m <= 1.0 / 2.0)
            {
                q = ((7.0 * m) - 3.0) / 2.0;
                r = 1.0;
                s = 1.0;
            }
            else if(1.0 / 2.0 <= m && m <= 4.0 / 7.0)
            {
                q = (7.0 * m) / 4.0;
                r = 0.0;
                s = 0.0;
            }
            else if(4.0 / 7.0 <= m && m <= 6.0 / 7.0)
            {
                q = 1.0;
                r = ((7.0 * m) - 4.0) / 2.0;
                s = 0.0;
            }
            else if (6.0 / 7.0 <= m && m <= 1)
            {
                q = 1.0;
                r = 1.0;
                s = (7.0 * m) - 6.0;
            }

            var x = ((input.R - m).Power(2) + (input.G - m).Power(2) + (input.B - m).Power(2)).SquareRoot();
            var y = (q - m).Power(2) + (r - m).Power(2) + (s - m).Power(2);

            saturation = x.SquareRoot() / y.SquareRoot();

            return new HSM(hue, saturation, mixture);
        }
    }
#pragma warning restore 1591
}

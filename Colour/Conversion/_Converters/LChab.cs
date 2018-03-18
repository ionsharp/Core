using Imagin.Colour.Primitives;
using Imagin.Common.Geometry;
using System;

namespace Imagin.Colour.Conversion
{
#pragma warning disable 1591
    public class LChabConverter : ColorConverterBase<LChab>
    {
        public LChab Convert(Lab input)
        {
            double L = input.L, a = input.a, b = input.b;

            var C = Math.Sqrt(a*a + b*b);
            var hRadians = Math.Atan2(b, a);
            var hDegrees = Angle.NormalizeDegree(Angle.GetDegree(hRadians));

            return new LChab(L, C, hDegrees, input.Illuminant);
        }
    }
#pragma warning restore 1591
}
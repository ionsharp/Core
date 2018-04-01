using Imagin.Colour.Primitives;
using Imagin.Common.Geometry;
using System;

namespace Imagin.Colour.Conversion
{
#pragma warning disable 1591
    public class LChuvConverter : ColorConverterBase<LChuv>
    {
        public LChuv Convert(Luv input)
        {
            double L = input.L, u = input.u, v = input.v;
            var C = Math.Sqrt(u*u + v*v);
            var hRadians = Math.Atan2(v, u);
            var hDegrees = Angle.NormalizeDegree(Angle.GetDegree(hRadians));

            return new LChuv(L, C, hDegrees, input.Illuminant);
        }
    }
#pragma warning restore 1591
}
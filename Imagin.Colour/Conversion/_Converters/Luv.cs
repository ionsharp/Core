using Imagin.Colour.Primitives;
using Imagin.Common.Geometry;
using System;

namespace Imagin.Colour.Conversion
{
#pragma warning disable 1591
    public class LuvConverter : ColorConverterBase<Luv>
    {
        public XYZ LuvWhitePoint
        {
            get;
        }

        public LuvConverter() : this(Luv.DefaultIlluminant) { }

        public LuvConverter(XYZ labWhitePoint) => LuvWhitePoint = labWhitePoint;

        static double Compute_up(XYZ input)
        {
            return (4 * input.X) / (input.X + 15 * input.Y + 3 * input.Z);
        }

        static double Compute_vp(XYZ input)
        {
            return (9 * input.Y) / (input.X + 15 * input.Y + 3 * input.Z);
        }

        /// ------------------------------------------------------------------------------------

        public Luv Convert(LChuv input)
        {
            double L = input.L, C = input.C, hDegrees = input.h;
            var hRadians = Angle.GetRadian(hDegrees);

            var u = C * Math.Cos(hRadians);
            var v = C * Math.Sin(hRadians);

            var output = new Luv(L, u, v, input.Illuminant);
            return output;
        }

        public Luv Convert(XYZ input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            var yr = input.Y/LuvWhitePoint.Y;
            var up = Compute_up(input);
            var vp = Compute_vp(input);
            var upr = Compute_up(LuvWhitePoint);
            var vpr = Compute_vp(LuvWhitePoint);

            var L = yr > CIE.Epsilon ? (116 * Math.Pow(yr, 1 / 3.0) - 16) : (CIE.Kappa * yr);

            if (double.IsNaN(L) || L < 0)
                L = 0;

            var u = 13*L*(up - upr);
            var v = 13*L*(vp - vpr);

            if (double.IsNaN(u))
                u = 0;

            if (double.IsNaN(v))
                v = 0;

            return new Luv(L, u, v, LuvWhitePoint);
        }
    }
#pragma warning restore 1591
}
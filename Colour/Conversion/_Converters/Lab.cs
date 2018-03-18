using Imagin.Colour.Primitives;
using Imagin.Common.Geometry;
using System;

namespace Imagin.Colour.Conversion
{
#pragma warning disable 1591
    public class LabConverter : ColorConverterBase<Lab>
    {
        public XYZ LabWhitePoint
        {
            get;
        }

        public LabConverter() : this(Lab.DefaultIlluminant) { }

        public LabConverter(XYZ labWhitePoint) => LabWhitePoint = labWhitePoint;

        static double F(double cr) => cr > CIE.Epsilon ? Math.Pow(cr, 1 / 3.0) : (CIE.Kappa * cr + 16.0) / 116.0;

        /// ------------------------------------------------------------------------------------

        public Lab Convert(LChab input)
        {
            double L = input.L, C = input.C, hDegrees = input.h;
            var hRadians = Angle.GetRadian(hDegrees);

            var a = C * Math.Cos(hRadians);
            var b = C * Math.Sin(hRadians);

            var output = new Lab(L, a, b, input.Illuminant);
            return output;
        }

        public Lab Convert(XYZ input)
        {
            double Xr = LabWhitePoint.X, Yr = LabWhitePoint.Y, Zr = LabWhitePoint.Z;

            double xr = input.X / Xr, yr = input.Y / Yr, zr = input.Z / Zr;

            var fx = F(xr);
            var fy = F(yr);
            var fz = F(zr);

            var L = 116 * fy - 16;
            var a = 500 * (fx - fy);
            var b = 200 * (fy - fz);

            var output = new Lab(L, a, b, LabWhitePoint);
            return output;
        }
    }
#pragma warning restore 1591
}
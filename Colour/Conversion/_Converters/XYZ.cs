using Imagin.Colour.Primitives;
using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Conversion
{
#pragma warning disable 1591
    public class XYZConverter : ColorConverterBase<XYZ>
    {
        readonly Matrix _conversionMatrix;

        public WorkingSpace SourceWorkingSpace
        {
            get;
        }

        public XYZConverter() { }

        public XYZConverter(WorkingSpace sourceWorkingSpace)
        {
            SourceWorkingSpace = sourceWorkingSpace;
            _conversionMatrix = WorkingSpace.GetRGBToXYZ(SourceWorkingSpace);
        }

        static double Compute_u0(XYZ input) => (4 * input.X) / (input.X + 15 * input.Y + 3 * input.Z);

        static double Compute_v0(XYZ input) => (9 * input.Y) / (input.X + 15 * input.Y + 3 * input.Z);

        /// ------------------------------------------------------------------------------------

        public XYZ Convert(HunterLab input)
        {
            double L = input.L, a = input.a, b = input.b;
            double Xn = input.Illuminant.X, Yn = input.Illuminant.Y, Zn = input.Illuminant.Z;

            var Ka = XYZ.ComputeKa(input.Illuminant);
            var Kb = XYZ.ComputeKb(input.Illuminant);

            var Y = (L / 100d).Power(2) * Yn;
            var X = ((a / Ka) * Math.Sqrt(Y / Yn) + Y / Yn) * Xn;
            var Z = ((b / Kb) * Math.Sqrt(Y / Yn) - Y / Yn) * (-Zn);

            var result = new XYZ(X, Y, Z);
            return result;
        }

        public XYZ Convert(Lab input)
        {
            double L = input.L, a = input.a, b = input.b;

            var fy = (L + 16) / 116d;
            var fx = a / 500d + fy;
            var fz = fy - b / 200d;

            var fx3 = fx.Power(3);
            var fz3 = fz.Power(3);

            var xr = fx3 > CIE.Epsilon ? fx3 : (116 * fx - 16) / CIE.Kappa;
            var yr = L > CIE.Kappa * CIE.Epsilon ? ((L + 16) / 116d).Power(3) : L / CIE.Kappa;
            var zr = fz3 > CIE.Epsilon ? fz3 : (116 * fz - 16) / CIE.Kappa;

            double Xr = input.Illuminant.X, Yr = input.Illuminant.Y, Zr = input.Illuminant.Z;

            //Avoids XYZ coordinates out range (restricted by 0 and XYZ reference white)
            xr = xr.Coerce(1, 0);
            yr = yr.Coerce(1, 0);
            zr = zr.Coerce(1, 0);

            var X = xr * Xr;
            var Y = yr * Yr;
            var Z = zr * Zr;

            var result = new XYZ(X, Y, Z);
            return result;
        }

        public XYZ Convert(LinearRGB input)
        {
            if (!Equals(input.WorkingSpace, SourceWorkingSpace))
                throw new InvalidOperationException("Working space of input RGB color must be equal to converter source RGB working space.");

            return new XYZ(_conversionMatrix.Multiply(input.Vector));
        }

        public XYZ Convert(Luv input)
        {
            double L = input.L, u = input.u, v = input.v;

            var u0 = Compute_u0(input.Illuminant);
            var v0 = Compute_v0(input.Illuminant);

            var Y = L > (CIE.Kappa * CIE.Epsilon)
                ? ((L + 16) / 116).Power(3)
                : (L / CIE.Kappa);

            var a = ((52 * L) / (u + 13 * L * u0) - 1) / 3;
            var b = -5 * Y;
            var c = -1 / 3d;
            var d = Y * ((39 * L) / (v + 13 * L * v0) - 5);

            var X = (d - b) / (a - c);
            var Z = X * a + b;

            if (double.IsNaN(X) || X < 0)
                X = 0;

            if (double.IsNaN(Y) || Y < 0)
                Y = 0;

            if (double.IsNaN(Z) || Z < 0)
                Z = 0;

            var result = new XYZ(X, Y, Z);
            return result;
        }

        public XYZ Convert(xyY input)
        {
            if (input.y == 0)
                return new XYZ(0, 0, input.Y);

            var X = (input.x * input.Y) / input.y;
            var Y = input.Y;
            var Z = ((1 - input.x - input.y) * Y) / input.y;

            return new XYZ(X, Y, Z);
        }
    }
#pragma warning restore 1591
}
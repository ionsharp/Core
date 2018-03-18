using Imagin.Colour.Primitives;
using System;

namespace Imagin.Colour.Conversion
{
#pragma warning disable 1591
    public class HunterLabConverter : ColorConverterBase<HunterLab>
    {
        public XYZ HunterLabWhitePoint
        {
            get;
        }

        public HunterLabConverter() : this(HunterLab.DefaultIlluminant) { }

        public HunterLabConverter(XYZ labWhitePoint) => HunterLabWhitePoint = labWhitePoint;

        /// ------------------------------------------------------------------------------------

        public HunterLab Convert(XYZ input)
        {
            // conversion algorithm described here: http://en.wikipedia.org/wiki/Lab_color_space#Hunter_Lab
            double X = input.X, Y = input.Y, Z = input.Z;
            double Xn = HunterLabWhitePoint.X, Yn = HunterLabWhitePoint.Y, Zn = HunterLabWhitePoint.Z;

            var Ka = XYZ.ComputeKa(HunterLabWhitePoint);
            var Kb = XYZ.ComputeKb(HunterLabWhitePoint);

            var L = 100 * Math.Sqrt(Y / Yn);
            var a = Ka * ((X/Xn - Y/Yn) / Math.Sqrt(Y/Yn));
            var b = Kb * ((Y/Yn - Z/Zn) / Math.Sqrt(Y/Yn));

            if (double.IsNaN(a))
                a = 0;

            if (double.IsNaN(b))
                b = 0;

            var output = new HunterLab(L, a, b, HunterLabWhitePoint);
            return output;
        }
    }
#pragma warning restore 1591
}
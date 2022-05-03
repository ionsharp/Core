using System;

namespace Imagin.Common.Colors
{
    public class sRGBCompanding : ICompanding
    {
        public sRGBCompanding() { }

        public double ConvertToLinear(double channel)
        {
            var V = channel;
            var v = V <= 0.04045 ? V / 12.92 : Math.Pow((V + 0.055) / 1.055, 2.4);
            return v;
        }

        public double ConvertToNonLinear(double channel)
        {
            var v = channel;
            var V = v <= 0.0031308 ? 12.92 * v : 1.055 * Math.Pow(v, 1 / 2.4d) - 0.055;
            return V;
        }
    }
}
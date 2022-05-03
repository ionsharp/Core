using System;

namespace Imagin.Common.Colors
{
    public class Rec2020Companding : ICompanding
    {
        public Rec2020Companding() { }

        public double ConvertToLinear(double channel)
        {
            var V = channel;
            var L = V < 0.08145 ? V / 4.5 : Math.Pow((V + 0.0993) / 1.0993, 1 / 0.45);
            return L;
        }

        public double ConvertToNonLinear(double channel)
        {
            var L = channel;
            var V = L < 0.0181 ? 4500 * L : 1.0993 * L - 0.0993;
            return V;
        }
    }
}
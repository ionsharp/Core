using System;

namespace Imagin.Common.Colors
{
    public class Rec709Companding : ICompanding
    {
        public Rec709Companding() { }

        public double ConvertToLinear(double channel)
        {
            var V = channel;
            var L = V < 0.081 ? V / 4.5 : Math.Pow((V + 0.099) / 1.099, 1 / 0.45);
            return L;
        }

        public double ConvertToNonLinear(double channel)
        {
            var L = channel;
            var V = L < 0.018 ? 4500 * L : 1.099 * L - 0.099;
            return V;
        }
    }
}
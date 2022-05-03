using Imagin.Common.Linq;
using System;

namespace Imagin.Common.Colors
{
    public class LCompanding : ICompanding
    {
        public LCompanding() { }

        public double ConvertToNonLinear(double channel)
        {
            var v = channel;
            var V = v <= CIE.Epsilon ? v * CIE.Kappa / 100d : Math.Pow(1.16 * v, 1 / 3d) - 0.16;
            return V;
        }

        public double ConvertToLinear(double channel)
        {
            var V = channel;
            var v = V <= 0.08 ? 100 * V / CIE.Kappa : ((V + 0.16) / 1.16).Power(3);
            return v;
        }
    }
}
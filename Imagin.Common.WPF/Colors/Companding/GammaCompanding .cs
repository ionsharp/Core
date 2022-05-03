using System;

namespace Imagin.Common.Colors
{
    public class GammaCompanding : ICompanding
    {
        public double Gamma { get; private set; }

        public GammaCompanding(double gamma) => Gamma = gamma;

        public double ConvertToNonLinear(double channel)
        {
            var v = channel;
            var V = Math.Pow(v, 1 / Gamma);
            return V;
        }

        public double ConvertToLinear(double channel)
        {
            var V = channel;
            var v = Math.Pow(V, Gamma);
            return v;
        }
    }
}
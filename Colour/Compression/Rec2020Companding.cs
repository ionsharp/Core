using System;

namespace Imagin.Colour.Compression
{
    /// <summary>
    /// 
    /// </summary>
    public class Rec2020Companding : ICompanding
    {
        /// <summary>
        /// 
        /// </summary>
        public Rec2020Companding() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public double InverseCompanding(double channel)
        {
            var V = channel;
            var L = V < 0.08145 ? V / 4.5 : Math.Pow((V + 0.0993) / 1.0993, 1 / 0.45);
            return L;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public double Companding(double channel)
        {
            var L = channel;
            var V = L < 0.0181 ? 4500 * L : 1.0993 * L - 0.0993;
            return V;
        }
    }
}

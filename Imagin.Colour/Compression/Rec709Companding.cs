using System;

namespace Imagin.Colour.Compression
{
    /// <summary>
    /// 
    /// </summary>
    public class Rec709Companding : ICompanding
    {
        /// <summary>
        /// 
        /// </summary>
        public Rec709Companding()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public double InverseCompanding(double channel)
        {
            var V = channel;
            var L = V < 0.081 ? V / 4.5 : Math.Pow((V + 0.099) / 1.099, 1 / 0.45);
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
            var V = L < 0.018 ? 4500 * L : 1.099 * L - 0.099;
            return V;
        }
    }
}

using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Compression
{
    /// <summary>
    /// 
    /// </summary>
    public class LCompanding : ICompanding
    {
        const double Kappa = 24389.0 / 27.0;

        const double Epsilon = 216.0 / 24389.0;

        /// <summary>
        /// 
        /// </summary>
        public LCompanding()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public double Companding(double channel)
        {
            var v = channel;
            var V = v <= Epsilon ? v * Kappa / 100d : Math.Pow(1.16 * v, 1 / 3d) - 0.16;
            return V;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public double InverseCompanding(double channel)
        {
            var V = channel;
            var v = V <= 0.08 ? 100 * V / Kappa : ((V + 0.16) / 1.16).Power(3);
            return v;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o)
        {
            if (ReferenceEquals(null, o))
                return false;

            if (ReferenceEquals(this, o))
                return true;

            if (o.GetType() != GetType())
                return false;

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => 1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(LCompanding left, LCompanding right) => Equals(left, right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(LCompanding left, LCompanding right) => !Equals(left, right);
    }
}

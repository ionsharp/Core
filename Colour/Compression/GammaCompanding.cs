using System;

namespace Imagin.Colour.Compression
{
    /// <summary>
    /// 
    /// </summary>
    public class GammaCompanding : ICompanding
    {
        readonly double _gamma;
        /// <summary>
        /// 
        /// </summary>
        public double Gamma => _gamma;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gamma"></param>
        public GammaCompanding(double gamma) => _gamma = gamma;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public double InverseCompanding(double channel)
        {
            var V = channel;
            var v = Math.Pow(V, Gamma);
            return v;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public double Companding(double channel)
        {
            var v = channel;
            var V = Math.Pow(v, 1 / Gamma);
            return V;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(GammaCompanding other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return _gamma.Equals(other._gamma);
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

            return Equals((GammaCompanding)o);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => _gamma.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(GammaCompanding left, GammaCompanding right) => Equals(left, right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(GammaCompanding left, GammaCompanding right) => !Equals(left, right);
    }
}

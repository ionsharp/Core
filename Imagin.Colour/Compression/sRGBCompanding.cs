using System;

namespace Imagin.Colour.Compression
{
    /// <summary>
    /// 
    /// </summary>
    public class sRGBCompanding : ICompanding
    {
        /// <summary>
        /// 
        /// </summary>
        public sRGBCompanding() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public double InverseCompanding(double channel)
        {
            var V = channel;
            var v = V <= 0.04045 ? V / 12.92 : Math.Pow((V + 0.055) / 1.055, 2.4);
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
            var V = v <= 0.0031308 ? 12.92 * v : 1.055 * Math.Pow(v, 1 / 2.4d) - 0.055;
            return V;
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
        public static bool operator ==(sRGBCompanding left, sRGBCompanding right) => Equals(left, right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(sRGBCompanding left, sRGBCompanding right) => !Equals(left, right);
    }
}

using System.Globalization;

namespace Imagin.Colour
{
    /// <summary>
    /// 
    /// </summary>
    public struct ChromacityPoint
    {
        readonly double _x;
        /// <summary>
        /// 
        /// </summary>
        public double X => _x;

        readonly double _y;
        /// <summary>
        /// 
        /// </summary>
        public double Y => _y;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public ChromacityPoint(double x, double y)
        {
            _x = x;
            _y = y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ChromacityPoint other) => _x.Equals(other._x) && _y.Equals(other._y);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o)
        {
            if (ReferenceEquals(null, o))
                return false;

            return o is ChromacityPoint && Equals((ChromacityPoint)o);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (_x.GetHashCode() * 397) ^ _y.GetHashCode();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(ChromacityPoint left, ChromacityPoint right) => left.Equals(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(ChromacityPoint left, ChromacityPoint right) => !left.Equals(right);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => string.Format(CultureInfo.InvariantCulture, "xy [x={0:0.##}, y={1:0.##}]", _x, _y);
    }
}

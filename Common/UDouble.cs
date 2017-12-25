using Imagin.Common.Linq;
using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public struct UDouble
    {
        /// <summary>
        /// Equivalent to <see cref="double.Epsilon"/>.
        /// </summary>
        public static UDouble Epsilon = double.Epsilon;

        /// <summary>
        /// Represents the smallest possible value of <see cref="UDouble"/> (0).
        /// </summary>
        public static UDouble MinValue = 0d;

        /// <summary>
        /// Represents the largest possible value of <see cref="UDouble"/> (equivalent to <see cref="double.MaxValue"/>).
        /// </summary>
        public static UDouble MaxValue = double.MaxValue;

        /// <summary>
        /// Equivalent to <see cref="double.NaN"/>.
        /// </summary>
        public static UDouble NaN = double.NaN;

        /// <summary>
        /// Equivalent to <see cref="double.PositiveInfinity"/>.
        /// </summary>
        public static UDouble PositiveInfinity = double.PositiveInfinity;

        double value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public UDouble(double Value)
        {
            if (double.IsNegativeInfinity(Value))
                throw new NotSupportedException();

            value = Value.Coerce(MaxValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        public static implicit operator double(UDouble d)
        {
            return d.value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        public static implicit operator UDouble(double d)
        {
            return new UDouble(d);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator <(UDouble a, UDouble b)
        {
            return a.value < b.value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >(UDouble a, UDouble b)
        {
            return a.value > b.value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(UDouble a, UDouble b)
        {
            return a.value == b.value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(UDouble a, UDouble b)
        {
            return a.value != b.value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator <=(UDouble a, UDouble b)
        {
            return a.value <= b.value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >=(UDouble a, UDouble b)
        {
            return a.value >= b.value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public override bool Equals(object a)
        {
            return a.IsNot<UDouble>() ? false : this == (UDouble)a;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return value.ToString();
        }
    }
}

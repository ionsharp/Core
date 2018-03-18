using Imagin.Common.Linq;
using System;

namespace Imagin.Common
{
    /// <summary>
    /// Represents an unsigned double-precision floating-point number.
    /// </summary>
    public struct UDouble
    {
        /// <summary>
        /// Equivalent to <see cref="double.Epsilon"/>.
        /// </summary>
        public readonly static UDouble Epsilon = double.Epsilon;

        /// <summary>
        /// Represents the largest possible value of <see cref="UDouble"/> (equivalent to <see cref="double.MaxValue"/>).
        /// </summary>
        public readonly static UDouble Maximum = double.MaxValue;

        /// <summary>
        /// Represents the smallest possible value of <see cref="UDouble"/> (0).
        /// </summary>
        public readonly static UDouble Minimum = 0;

        /// <summary>
        /// Equivalent to <see cref="double.NaN"/>.
        /// </summary>
        public readonly static UDouble NaN = double.NaN;

        /// <summary>
        /// Equivalent to <see cref="double.PositiveInfinity"/>.
        /// </summary>
        public readonly static UDouble PositiveInfinity = double.PositiveInfinity;

        double _value;

        /// <summary>
        /// Initializes an instance of the <see cref="UDouble"/> structure.
        /// </summary>
        /// <param name="Value"></param>
        public UDouble(double Value)
        {
            if (double.IsNegativeInfinity(Value))
                throw new NotSupportedException();

            _value = Value.Coerce(Maximum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        public static implicit operator double(UDouble d)
        {
            return d._value;
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
            return a._value < b._value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >(UDouble a, UDouble b)
        {
            return a._value > b._value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(UDouble a, UDouble b)
        {
            return a._value == b._value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(UDouble a, UDouble b)
        {
            return a._value != b._value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator <=(UDouble a, UDouble b)
        {
            return a._value <= b._value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >=(UDouble a, UDouble b)
        {
            return a._value >= b._value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static UDouble operator +(UDouble a, UDouble b)
        {
            return a._value + b._value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static UDouble operator -(UDouble a, UDouble b)
        {
            return a._value - b._value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static UDouble operator /(UDouble a, UDouble b)
        {
            return a._value / b._value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static UDouble operator *(UDouble a, UDouble b)
        {
            return a._value * b._value;
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
            return _value.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UDouble Parse(string value)
        {
            return double.Parse(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(string value, out UDouble result)
        {
            var r = double.TryParse(value, out double d);
            result = d;
            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string ToString(string format)
        {
            return _value.ToString(format);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _value.ToString();
        }
    }
}

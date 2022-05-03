using Imagin.Common.Linq;
using System;

namespace Imagin.Common
{
    /// <summary>
    /// Represents an unsigned double-precision floating-point number.
    /// </summary>
    [Serializable]
    public struct UDouble
    {
        /// <summary>
        /// Equivalent to <see cref="double.Epsilon"/>.
        /// </summary>
        public readonly static UDouble Epsilon = double.Epsilon;

        /// <summary>
        /// Represents the largest possible value of <see cref="UDouble"/> (equivalent to <see cref="double.MaxValue"/>).
        /// </summary>
        public readonly static UDouble MaxValue = double.MaxValue;

        /// <summary>
        /// Represents the smallest possible value of <see cref="UDouble"/> (0).
        /// </summary>
        public readonly static UDouble MinValue = 0;

        /// <summary>
        /// Equivalent to <see cref="double.NaN"/>.
        /// </summary>
        public readonly static UDouble NaN = double.NaN;

        /// <summary>
        /// Equivalent to <see cref="double.PositiveInfinity"/>.
        /// </summary>
        public readonly static UDouble PositiveInfinity = double.PositiveInfinity;
        readonly double value;

        /// <summary>
        /// Initializes an instance of the <see cref="UDouble"/> structure.
        /// </summary>
        /// <param name="input"></param>
        public UDouble(double input)
        {
            if (double.IsNegativeInfinity(input))
                throw new NotSupportedException();

            value = input.Coerce(double.MaxValue);
        }

        public static implicit operator double(UDouble d) => d.value;

        public static implicit operator UDouble(double d) => new UDouble(d);

        public static bool operator <(UDouble a, UDouble b) => a.value < b.value;

        public static bool operator >(UDouble a, UDouble b) => a.value > b.value;

        public static bool operator ==(UDouble a, UDouble b) => a.value == b.value;

        public static bool operator !=(UDouble a, UDouble b) => a.value != b.value;

        public static bool operator <=(UDouble a, UDouble b) => a.value <= b.value;

        public static bool operator >=(UDouble a, UDouble b) => a.value >= b.value;

        public static UDouble operator +(UDouble a, UDouble b) => a.value + b.value;

        public static UDouble operator -(UDouble a, UDouble b) => a.value - b.value;

        public static UDouble operator /(UDouble a, UDouble b) => a.value / b.value;

        public static UDouble operator *(UDouble a, UDouble b) => a.value * b.value;

        public override bool Equals(object a) => a is UDouble b ? this == b : false;

        public override int GetHashCode() => value.GetHashCode();

        public static UDouble Parse(string input) => double.Parse(input);

        public static bool TryParse(string input, out UDouble result)
        {
            var r = double.TryParse(input, out double d);
            result = d;
            return r;
        }

        public string ToString(string format) => value.ToString(format);

        public override string ToString() => value.ToString();
    }
}

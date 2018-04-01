using Imagin.Common.Media;
using System;
using Imagin.Common.Geometry;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        public static Double Add(this Double value, Double increment)
            => value + increment;

        /// <summary>
        /// Gets the absolute value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Double Absolute(this Double value)
            => Math.Abs(value);

        /// <summary>
        /// Rounds up to the nearest whole number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Double Ceiling(this Double value)
            => Math.Ceiling(value);

        /// <summary>
        /// Coerces to the specified range.
        /// </summary>
        /// <param name="value">The value to coerce.</param>
        /// <param name="maximum">The maximum to coerce to.</param>
        /// <param name="minimum">The minimum to coerce to.</param>
        /// <returns></returns>
        public static Double Coerce(this Double value, Double maximum, Double minimum = 0)
            => Math.Max(Math.Min(value, maximum), minimum);

        /// <summary>
        /// Gets the cubic root.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Double CubicRoot(this Double value) 
            => value.Power(1.0 / 3.0);

        /// <summary>
        /// Divides by the given divisor.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static Double Divide(this Double value, Double divisor) 
            => value / divisor;

        /// <summary>
        /// Rounds down to the nearest whole number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Double Floor(this Double value) 
            => Math.Floor(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Double Modulo(this Double value, Double left, Double? right = null)
        {
            //Detect single-argument case
            if (right == null)
            {
                right = left;
                left = 0;
            }

            //Swap frame order
            if (left > right)
            {
                var tmp = right.Value;
                right = left;
                left = tmp;
            }

            var frame = right.Value - left;
            value = ((value + left) % frame) - left;

            if (value < left)
                value += frame;

            if (value > right)
                value -= frame;

            return value;
        }

        /// <summary>
        /// Multiplies by the given scalar.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public static Double Multiply(this Double value, Double scalar) 
            => value * scalar;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        public static Double NearestFactor(this Double value, Double factor)
            => Math.Round((value / factor), MidpointRounding.AwayFromZero) * factor;

        /// <summary>
        /// Raises to the specified power.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public static Double Power(this Double value, Double power) 
            => Math.Pow(value, power);

        /// <summary>
        /// Rounds to the given digits.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static Double Round(this Double value, Int32 digits = 0) 
            => Math.Round(value, digits);

        /// <summary>
        /// Shifts the decimal point. <para>If negative, shift left; otherwise, shift right.</para>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="shifts"></param>
        /// <returns></returns>
        public static Double Shift(this Double value, Int32 shifts = 1)
        {
            var LeftOrRight = shifts < 0;

            for (var i = 0; i < shifts.Absolute(); i++)
                value = LeftOrRight ? value / 10 : value * 10;

            return value;
        }

        /// <summary>
        /// Shifts the decimal point and rounds to the given digits. <para>If negative, shift left; otherwise, shift right.</para>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="shifts"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static Double ShiftRound(this Double value, Int32 shifts = 1, Int32 digits = 0) 
            => value.Shift(shifts).Round(digits);

        /// <summary>
        /// Gets the square root.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Double SquareRoot(this Double value) 
            => Math.Sqrt(value);

        /// <summary>
        /// Subtracts by the given decrement.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decrement"></param>
        /// <returns></returns>
        public static Double Subtract(this Double value, Double decrement) 
            => value - decrement;

        /// <summary>
        /// Converts to <see cref="byte"/>.
        /// </summary>
        public static Byte ToByte(this Double value)
            => Convert.ToByte(value);

        /// <summary>
        /// Converts to <see cref="float"/>.
        /// </summary>
        public static Decimal ToDecimal(this Double value) 
            => Convert.ToDecimal(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int16 ToInt16(this Double value) 
            => Convert.ToInt16(value);

        /// <summary>
        /// Converts to <see cref="int"/>.
        /// </summary>
        public static Int32 ToInt32(this Double value)
            => Convert.ToInt32(value);

        /// <summary>
        /// Converts to <see cref="long"/>.
        /// </summary>
        public static Int64 ToInt64(this Double value) 
            => Convert.ToInt64(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Single ToSingle(this Double value) 
            => Convert.ToSingle(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt16 ToUInt16(this Double value) 
            => Convert.ToUInt16(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt32 ToUInt32(this Double value) 
            => Convert.ToUInt32(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt64 ToUInt64(this Double value)
            => Convert.ToUInt64(value);

        /// <summary>
        /// Converts a graphical unit value to another graphical unit value.
        /// </summary>
        /// <param name="value">Number of units.</param>
        /// <param name="from">The unit to convert from.</param>
        /// <param name="to">The unit to convert to.</param>
        /// <param name="ppi">Pixels per inch.</param>
        public static Double ToUnit(this Double value, GraphicalUnit from, GraphicalUnit to, Double ppi = 72.0)
        {
            var pixels = 0d;
            switch (from)
            {
                case GraphicalUnit.Pixel:
                    pixels = Math.Round(value, 0);
                    break;
                case GraphicalUnit.Inch:
                    pixels = Math.Round(value * ppi, 0);
                    break;
                case GraphicalUnit.Centimeter:
                    pixels = Math.Round((value * ppi) / 2.54, 0);
                    break;
                case GraphicalUnit.Millimeter:
                    pixels = Math.Round((value * ppi) / 25.4, 0);
                    break;
                case GraphicalUnit.Point:
                    pixels = Math.Round((value * ppi) / 72, 0);
                    break;
                case GraphicalUnit.Pica:
                    pixels = Math.Round((value * ppi) / 6, 0);
                    break;
                case GraphicalUnit.Twip:
                    pixels = Math.Round((value * ppi) / 1140, 0);
                    break;
                case GraphicalUnit.Character:
                    pixels = Math.Round((value * ppi) / 12, 0);
                    break;
                case GraphicalUnit.En:
                    pixels = Math.Round((value * ppi) / 144.54, 0);
                    break;
            }

            var inches = pixels / ppi;
            var result = pixels;

            switch (to)
            {
                case GraphicalUnit.Inch:
                    result = inches;
                    break;
                case GraphicalUnit.Centimeter:
                    result = inches * 2.54;
                    break;
                case GraphicalUnit.Millimeter:
                    result = inches * 25.4;
                    break;
                case GraphicalUnit.Point:
                    result = inches * 72.0;
                    break;
                case GraphicalUnit.Pica:
                    result = inches * 6.0;
                    break;
                case GraphicalUnit.Twip:
                    result = inches * 1140.0;
                    break;
                case GraphicalUnit.Character:
                    result = inches * 12.0;
                    break;
                case GraphicalUnit.En:
                    result = inches * 144.54;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Gets whether or not the <see cref="double"/> is in the specified range.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="minimum">The minimum value in the range.</param>
        /// <param name="maximum">The maximum value in the range.</param>
        /// <returns></returns>
        public static bool Within(this Double value, Double minimum, Double maximum) 
            => value >= minimum && value <= maximum;
    }
}

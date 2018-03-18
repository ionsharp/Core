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
        public static double Add(this double value, double increment)
        {
            return value + increment;
        }

        /// <summary>
        /// Gets the absolute value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double Absolute(this double value)
        {
            return Math.Abs(value);
        }

        /// <summary>
        /// Rounds up to the nearest whole number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double Ceiling(this double value)
        {
            return Math.Ceiling(value);
        }

        /// <summary>
        /// Coerces to the specified range.
        /// </summary>
        /// <param name="value">The value to coerce.</param>
        /// <param name="maximum">The maximum to coerce to.</param>
        /// <param name="minimum">The minimum to coerce to.</param>
        /// <returns></returns>
        public static double Coerce(this double value, double maximum, double minimum = 0) => Math.Max(Math.Min(value, maximum), minimum);

        /// <summary>
        /// Gets the cubic root.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double CubicRoot(this double value) => value.Power(1.0 / 3.0);

        /// <summary>
        /// Divides by the given divisor.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static double Divide(this double value, double divisor) => value / divisor;

        /// <summary>
        /// Rounds down to the nearest whole number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double Floor(this double value) => Math.Floor(value);

        /// <summary>
        /// Gets the degree of the given radian.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double GetDegree(this double value) => Angle.GetDegree(value);

        /// <summary>
        /// Gets the radian of the given degree.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double GetRadian(this double value) => Angle.GetRadian(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static double Modulo(this double value, double left, double? right = null)
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
        public static double Multiply(this double value, double scalar) => value * scalar;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        public static double NearestFactor(this double value, double factor)
        {
            return Math.Round((value / factor), MidpointRounding.AwayFromZero) * factor;
        }

        /// <summary>
        /// Raises to the specified power.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public static double Power(this double value, double power) => Math.Pow(value, power);

        /// <summary>
        /// Rounds to the given digits.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static double Round(this double value, int digits = 0) => Math.Round(value, digits);

        /// <summary>
        /// Shifts the decimal point. <para>If negative, shift left; otherwise, shift right.</para>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="shifts"></param>
        /// <returns></returns>
        public static double Shift(this double value, int shifts = 1)
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
        public static double ShiftRound(this double value, int shifts = 1, int digits = 0) => value.Shift(shifts).Round(digits);

        /// <summary>
        /// Gets the square root.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double SquareRoot(this double value) => Math.Sqrt(value);

        /// <summary>
        /// Subtracts by the given decrement.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decrement"></param>
        /// <returns></returns>
        public static double Subtract(this double value, double decrement) => value - decrement;

        /// <summary>
        /// Converts to <see cref="byte"/>.
        /// </summary>
        public static byte ToByte(this double value) => Convert.ToByte(value);

        /// <summary>
        /// Converts to <see cref="float"/>.
        /// </summary>
        public static float ToFloat(this double value) => Convert.ToSingle(value);

        /// <summary>
        /// Converts to <see cref="int"/>.
        /// </summary>
        public static int ToInt32(this double value) => Convert.ToInt32(value);

        /// <summary>
        /// Converts to <see cref="long"/>.
        /// </summary>
        public static long ToInt64(this double value) => Convert.ToInt64(value);

        /// <summary>
        /// Converts a graphical unit value to another graphical unit value.
        /// </summary>
        /// <param name="value">Number of units.</param>
        /// <param name="from">The unit to convert from.</param>
        /// <param name="to">The unit to convert to.</param>
        /// <param name="Ppi">Pixels per inch.</param>
        public static double ToUnit(this double value, GraphicalUnit from, GraphicalUnit to, double Ppi = 72.0)
        {
            //Convert to pixels
            double Pixels = 0.0;
            switch (from)
            {
                case GraphicalUnit.Pixel:
                    Pixels = Math.Round(value, 0);
                    break;
                case GraphicalUnit.Inch:
                    Pixels = Math.Round(value * Ppi, 0);
                    break;
                case GraphicalUnit.Centimeter:
                    Pixels = Math.Round((value * Ppi) / 2.54, 0);
                    break;
                case GraphicalUnit.Millimeter:
                    Pixels = Math.Round((value * Ppi) / 25.4, 0);
                    break;
                case GraphicalUnit.Point:
                    Pixels = Math.Round((value * Ppi) / 72, 0);
                    break;
                case GraphicalUnit.Pica:
                    Pixels = Math.Round((value * Ppi) / 6, 0);
                    break;
                case GraphicalUnit.Twip:
                    Pixels = Math.Round((value * Ppi) / 1140, 0);
                    break;
                case GraphicalUnit.Character:
                    Pixels = Math.Round((value * Ppi) / 12, 0);
                    break;
                case GraphicalUnit.En:
                    Pixels = Math.Round((value * Ppi) / 144.54, 0);
                    break;
            }

            double Inches = Pixels / Ppi;

            double Result = Pixels;

            //Convert to target unit
            switch (to)
            {
                case GraphicalUnit.Inch:
                    Result = Inches;
                    break;
                case GraphicalUnit.Centimeter:
                    Result = Inches * 2.54;
                    break;
                case GraphicalUnit.Millimeter:
                    Result = Inches * 25.4;
                    break;
                case GraphicalUnit.Point:
                    Result = Inches * 72.0;
                    break;
                case GraphicalUnit.Pica:
                    Result = Inches * 6.0;
                    break;
                case GraphicalUnit.Twip:
                    Result = Inches * 1140.0;
                    break;
                case GraphicalUnit.Character:
                    Result = Inches * 12.0;
                    break;
                case GraphicalUnit.En:
                    Result = Inches * 144.54;
                    break;
            }

            return Result;
        }

        /// <summary>
        /// Gets whether or not the <see cref="double"/> is in the specified range.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="minimum">The minimum value in the range.</param>
        /// <param name="maximum">The maximum value in the range.</param>
        /// <returns></returns>
        public static bool Within(this double value, double minimum, double maximum) => value >= minimum && value <= maximum;
    }
}

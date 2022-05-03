using Imagin.Common.Media;
using Imagin.Common.Numbers;
using System;

namespace Imagin.Common.Linq
{
    public static class XDouble
    {
        /// <summary>
        /// Gets a <see cref="TimeSpan"/> representing the given number of days.
        /// </summary>
        public static TimeSpan Days(this double input) => TimeSpan.FromDays(input);

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> representing the given number of hours.
        /// </summary>
        public static TimeSpan Hours(this double input) => TimeSpan.FromHours(input);

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> representing the given number of milliseconds.
        /// </summary>
        public static TimeSpan Milliseconds(this double input) => TimeSpan.FromMilliseconds(input);

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> representing the given number of minutes.
        /// </summary>
        public static TimeSpan Minutes(this double input) => TimeSpan.FromMinutes(input);

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> representing the given number of months.
        /// </summary>
        public static TimeSpan Months(this double input) => TimeSpan.FromDays(30 * input);

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> representing the given number of seconds.
        /// </summary>
        public static TimeSpan Seconds(this double input) => TimeSpan.FromSeconds(input);

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> representing the given number of years.
        /// </summary>
        public static TimeSpan Years(this double input) => TimeSpan.FromDays(365 * input);

        //...

        public static double Add(this double value, double increment)
            => value + increment;

        /// <summary>
        /// Gets the absolute value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double Absolute(this double value)
            => Math.Abs(value);

        /// <summary>
        /// Converts to <see cref="byte"/>.
        /// </summary>
        public static byte Byte(this double value) => System.Convert.ToByte(value);

        /// <summary>
        /// Rounds up to the nearest whole number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double Ceiling(this double value)
            => Math.Ceiling(value);

        /// <summary>
        /// Coerces to the specified range.
        /// </summary>
        /// <param name="i">The value to coerce.</param>
        /// <param name="maximum">The maximum to coerce to.</param>
        /// <param name="minimum">The minimum to coerce to.</param>
        /// <returns></returns>
        public static double Coerce(this double i, double maximum, double minimum = 0)
            => Math.Max(Math.Min(i, maximum), minimum);

        /// <summary>
        /// Converts a value to the given <see cref="GraphicUnit"/> (to) based on the given <see cref="GraphicUnit"/> (from).
        /// </summary>
        /// <param name="value">Number of units.</param>
        /// <param name="from">The unit to convert from.</param>
        /// <param name="to">The unit to convert to.</param>
        /// <param name="ppi">Pixels per inch.</param>
        public static double Convert(this double value, GraphicUnit from, GraphicUnit to, double ppi = 72.0)
        {
            var pixels = 0d;
            switch (from)
            {
                case GraphicUnit.Pixel:
                    pixels = Math.Round(value, 0);
                    break;
                case GraphicUnit.Inch:
                    pixels = Math.Round(value * ppi, 0);
                    break;
                case GraphicUnit.Centimeter:
                    pixels = Math.Round((value * ppi) / 2.54, 0);
                    break;
                case GraphicUnit.Millimeter:
                    pixels = Math.Round((value * ppi) / 25.4, 0);
                    break;
                case GraphicUnit.Point:
                    pixels = Math.Round((value * ppi) / 72, 0);
                    break;
                case GraphicUnit.Pica:
                    pixels = Math.Round((value * ppi) / 6, 0);
                    break;
                case GraphicUnit.Twip:
                    pixels = Math.Round((value * ppi) / 1140, 0);
                    break;
                case GraphicUnit.Character:
                    pixels = Math.Round((value * ppi) / 12, 0);
                    break;
                case GraphicUnit.En:
                    pixels = Math.Round((value * ppi) / 144.54, 0);
                    break;
            }

            var inches = pixels / ppi;
            var result = pixels;

            switch (to)
            {
                case GraphicUnit.Inch:
                    result = inches;
                    break;
                case GraphicUnit.Centimeter:
                    result = inches * 2.54;
                    break;
                case GraphicUnit.Millimeter:
                    result = inches * 25.4;
                    break;
                case GraphicUnit.Point:
                    result = inches * 72.0;
                    break;
                case GraphicUnit.Pica:
                    result = inches * 6.0;
                    break;
                case GraphicUnit.Twip:
                    result = inches * 1140.0;
                    break;
                case GraphicUnit.Character:
                    result = inches * 12.0;
                    break;
                case GraphicUnit.En:
                    result = inches * 144.54;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Gets the cubic root.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double CubicRoot(this double value) 
            => value.Power(1.0 / 3.0);

        /// <summary>
        /// Converts to <see cref="float"/>.
        /// </summary>
        public static decimal Decimal(this double value)
            => System.Convert.ToDecimal(value);

        /// <summary>
        /// Divides by the given divisor.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static double Divide(this double value, double divisor) 
            => value / divisor;

        public static bool EqualsAny(this double input, params double[] values)
        {
            foreach (var i in values)
            {
                if (input == i)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Rounds down to the nearest whole number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double Floor(this double value) 
            => Math.Floor(value);

        public static double GetDegree(this double input)
            => Angle.GetDegree(input);

        public static double GetRadian(this double input)
            => Angle.GetRadian(input);

        public static short Int16(this double value)
            => System.Convert.ToInt16(value);

        /// <summary>
        /// Converts to <see cref="int"/>.
        /// </summary>
        public static int Int32(this double value)
            => System.Convert.ToInt32(value);

        /// <summary>
        /// Converts to <see cref="long"/>.
        /// </summary>
        public static long Int64(this double value)
            => System.Convert.ToInt64(value);

        public static double Maximum(this double input, double maximum)
            => input.Coerce(maximum, double.MinValue);

        public static double Minimum(this double input, double minimum)
            => input.Coerce(double.MaxValue, minimum);

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

        public static byte Multiply(this double input, byte b)
            => (input * b.Double()).Round().Coerce(byte.MaxValue).Byte();

        public static double Multiply(this double a, double b)
            => a * b;

        public static double Negate(this double value)
            => -value;

        public static double NearestFactor(this double value, double factor)
            => Math.Round((value / factor), MidpointRounding.AwayFromZero) * factor;

        /// <summary>
        /// Raises to the specified power.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public static double Power(this double value, double power) 
            => Math.Pow(value, power);

        /// <summary>
        /// Rounds to the given digits.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static double Round(this double value, int digits = 0) 
            => Math.Round(value, digits);

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
        public static double ShiftRound(this double value, int shifts = 1, int digits = 0) 
            => value.Shift(shifts).Round(digits);

        public static float Single(this double value)
            => System.Convert.ToSingle(value);

        /// <summary>
        /// Gets the square root.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double SquareRoot(this double value) 
            => Math.Sqrt(value);

        /// <summary>
        /// Subtracts by the given decrement.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decrement"></param>
        /// <returns></returns>
        public static double Subtract(this double value, double decrement) 
            => value - decrement;

        public static ushort UInt16(this double value)
            => System.Convert.ToUInt16(value);

        public static uint UInt32(this double value) 
            => System.Convert.ToUInt32(value);

        public static ulong UInt64(this double value)
            => System.Convert.ToUInt64(value);

        public static bool Within(this double input, double minimum, double maximum) => input >= minimum && input <= maximum;
    }
}
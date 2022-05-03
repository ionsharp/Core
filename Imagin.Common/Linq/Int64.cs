using Imagin.Common.Data;
using System;

namespace Imagin.Common.Linq
{
    public static class XInt64
    {
        public static long Add(this long value, long increment) => value + increment;

        public static long Absolute(this long value) => Math.Abs(value);

        public static long Coerce(this long value, long maximum, long minimum = 0L) => Math.Max(Math.Min(value, maximum), minimum);

        public static long Divide(this long value, long divisor) => value / divisor;

        public static double Double(this long value) => Convert.ToDouble(value);

        public static string FileSize(this long value, FileSizeFormat format, int round = 1) => value.Coerce(long.MaxValue).UInt64().FileSize(format, round);

        public static short Int16(this long value) => Convert.ToInt16(value);

        public static int Int32(this long value) => Convert.ToInt32(value);

        public static long K(this long value) => value * 1024L;

        public static long M(this long value) => value * 1024L * 1024L;

        public static long Maximum(this long input, long maximum) => input.Coerce(maximum, long.MinValue);

        public static long Minimum(this long input, long minimum) => input.Coerce(long.MaxValue, minimum);

        public static long Multiply(this long value, long scalar) => value * scalar;

        public static long Negate(this long value) => -value;

        public static float Single(this long value) => Convert.ToSingle(value);

        public static long Subtract(this long value, long decrement) => value - decrement;

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> representing the given number of ticks.
        /// </summary>
        public static TimeSpan Ticks(this long input) => TimeSpan.FromTicks(input);

        public static ushort UInt16(this long value) => Convert.ToUInt16(value);

        public static uint UInt32(this long value) => Convert.ToUInt32(value);

        public static ulong UInt64(this long value) => Convert.ToUInt64(value);

        public static bool Within(this long input, long minimum, long maximum) => input >= minimum && input <= maximum;
    }
}
using Imagin.Common.Data;
using Imagin.Common.Linq;
using System;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class Int64Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        public static long Add(this long value, long increment) => value + increment;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long Absolute(this long value) => Math.Abs(value);

        /// <summary>
        /// Coerces to the specified range.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maximum"></param>
        /// <param name="minimum"></param>
        /// <returns></returns>
        public static long Coerce(this long value, long maximum, long minimum = 0L) => Math.Max(Math.Min(value, maximum), minimum);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static long Divide(this long value, long divisor) => value / divisor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long K(this long value) => value * 1024L;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long M(this long value) => value * 1024L * 1024L;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public static long Multiply(this long value, long scalar) => value * scalar;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decrement"></param>
        /// <returns></returns>
        public static long Subtract(this long value, long decrement) => value - decrement;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(this long value) => Convert.ToDouble(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <param name="round"></param>
        /// <returns></returns>
        public static string ToFileSize(this long value, FileSizeFormat format, int round = 1) => value.Coerce(long.MaxValue).ToUInt64().ToFileSize(format, round);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToInt32(this long value) => Convert.ToInt32(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ulong ToUInt64(this long value) => Convert.ToUInt64(value);
    }
}

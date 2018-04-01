using System;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        /// Coerces to the specified range.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maximum"></param>
        /// <param name="minimum"></param>
        /// <returns></returns>
        public static byte Coerce(this byte value, byte maximum, byte minimum = 0) => Math.Max(Math.Min(value, maximum), minimum);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(this byte value)
        {
            return Convert.ToDouble(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt32(this byte value)
        {
            return Convert.ToInt32(value);
        }
    }
}

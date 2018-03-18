using System;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class Int16Extensions
    {
        /// <summary>
        /// Coerces to the specified range.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static short Abs(this short value)
        {
            return Math.Abs(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maximum"></param>
        /// <param name="minimum"></param>
        /// <returns></returns>
        public static short Coerce(this short value, short maximum, short minimum = 0) => Math.Max(Math.Min(value, maximum), minimum);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt32(this short value)
        {
            return Convert.ToInt32(value);
        }

    }
}

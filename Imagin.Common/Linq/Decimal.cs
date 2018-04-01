using System;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class DecimalExtensions
    {
        /// <summary>
        /// Coerces to the specified range.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maximum"></param>
        /// <param name="minimum"></param>
        /// <returns></returns>
        public static decimal Coerce(this decimal value, decimal maximum, decimal minimum = 0) => Math.Max(Math.Min(value, maximum), minimum);
    }
}

using System;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class UInt16Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maximum"></param>
        /// <param name="minimum"></param>
        /// <returns></returns>
        public static ushort Coerce(this ushort value, ushort maximum, ushort minimum = 0) => Math.Max(Math.Min(value, maximum), minimum);
    }
}

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
        public static ushort Coerce(this UInt16 value, UInt16 maximum, UInt16 minimum = 0) 
            => Math.Max(Math.Min(value, maximum), minimum);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Decimal ToDecimal(this UInt16 value)
            => Convert.ToDecimal(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Double ToDouble(this UInt16 value)
            => Convert.ToDouble(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int16 ToInt16(this UInt16 value)
            => Convert.ToInt16(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int32 ToInt32(this UInt16 value)
            => Convert.ToInt32(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int64 ToInt64(this UInt16 value)
            => Convert.ToInt64(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Single ToSingle(this UInt16 value)
            => Convert.ToSingle(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt32 ToUInt32(this UInt16 value)
            => Convert.ToUInt32(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt64 ToUInt64(this UInt16 value)
            => Convert.ToUInt64(value);
    }
}
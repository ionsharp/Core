using System;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class SingleExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Single Absolute(this Single value) 
            => Math.Abs(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maximum"></param>
        /// <param name="minimum"></param>
        /// <returns></returns>
        public static Single Coerce(this Single value, Single maximum, Single minimum = 0f) 
            => Math.Max(Math.Min(value, maximum), minimum);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Decimal ToDecimal(this Single value)
            => Convert.ToDecimal(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Double ToDouble(this Single value)
            => Convert.ToDouble(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int16 ToInt16(this Single value)
            => Convert.ToInt16(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int32 ToInt32(this Single value)
            => Convert.ToInt32(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int64 ToInt64(this Single value)
            => Convert.ToInt64(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt16 ToUInt16(this Single value)
            => Convert.ToUInt16(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt32 ToUInt32(this Single value)
            => Convert.ToUInt32(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt64 ToUInt64(this Single value)
            => Convert.ToUInt64(value);
    }
}
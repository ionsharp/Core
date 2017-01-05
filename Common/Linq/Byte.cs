using System;

namespace Imagin.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ToCoerce"></param>
        /// <param name="Maximum"></param>
        /// <param name="Minimum"></param>
        /// <returns></returns>
        public static byte Coerce(this byte ToCoerce, byte Maximum, byte Minimum = (byte)0)
        {
            return ToCoerce > Maximum ? Maximum : (ToCoerce < Minimum ? Minimum : ToCoerce);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static double ToDouble(this byte Value)
        {
            return Convert.ToDouble(Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static int ToInt32(this byte Value)
        {
            return Convert.ToInt32(Value);
        }
    }
}

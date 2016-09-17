using System;

namespace Imagin.Common.Extensions
{
    public static class ByteExtensions
    {
        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static byte Coerce(this byte ToCoerce, byte Maximum, byte Minimum = (byte)0)
        {
            return ToCoerce > Maximum ? Maximum : (ToCoerce < Minimum ? Minimum : ToCoerce);
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static double ToDouble(this byte ToConvert)
        {
            return Convert.ToDouble(ToConvert);
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static int ToInt(this byte ToConvert)
        {
            return Convert.ToInt32(ToConvert);
        }
    }
}

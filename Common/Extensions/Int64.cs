using System;

namespace Imagin.Common.Extensions
{
    public static class Int64Extensions
    {
        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static long Coerce(this long ToCoerce, long Maximum, long Minimum = 0L)
        {
            return ToCoerce > Maximum ? Maximum : (ToCoerce < Minimum ? Minimum : ToCoerce);
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static string ToFileSize(this long Bytes)
        {
            return Convert.ToDouble(Bytes).ToFileSize();
        }
    }
}

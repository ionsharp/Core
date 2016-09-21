using System;

namespace Imagin.Common.Extensions
{
    public static class Int64Extensions
    {
        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static long Add(this long ToAdd, long Increment)
        {
            return ToAdd + Increment;
        }

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
        public static long Divide(this long ToDivide, long Divisor)
        {
            return ToDivide / Divisor;
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static long Multiply(this long ToMultiply, long Scalar)
        {
            return ToMultiply * Scalar;
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static long Subtract(this long ToSubtract, long Decrement)
        {
            return ToSubtract - Decrement;
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

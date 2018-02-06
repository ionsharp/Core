using System;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class FloatExtensions
    {
        /// <summary>
        /// Coerces <see cref="float"/> to given maximum and minimum.
        /// </summary>
        /// <param name="Value">The value to coerce.</param>
        /// <param name="Maximum">The maximum to coerce to.</param>
        /// <param name="Minimum">The minimum to coerce to.</param>
        /// <returns></returns>
        public static float Coerce(this float Value, float Maximum, float Minimum = 0f)
        {
            return Value > Maximum ? Maximum : (Value < Minimum ? Minimum : Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ToConvert"></param>
        /// <returns></returns>
        public static int ToInt(this float ToConvert)
        {
            return Convert.ToInt32(ToConvert);
        }
    }
}

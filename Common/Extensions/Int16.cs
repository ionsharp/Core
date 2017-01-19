using System;

namespace Imagin.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class Int16Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static short Abs(this short Value)
        {
            return Math.Abs(Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Maximum"></param>
        /// <param name="Minimum"></param>
        /// <returns></returns>
        public static short Coerce(this short Value, short Maximum, short Minimum = 0)
        {
            return Value > Maximum ? Maximum : (Value < Minimum ? Minimum : Value);
        }
    }
}

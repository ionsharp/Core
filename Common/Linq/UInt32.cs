using System;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class UInt32Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Maximum"></param>
        /// <param name="Minimum"></param>
        /// <returns></returns>
        public static uint Coerce(this uint Value, uint Maximum, uint Minimum = 0)
        {
            return Value > Maximum ? Maximum : (Value < Minimum ? Minimum : Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static int ToInt32(this uint Value)
        {
            return Convert.ToInt32(Value);
        }
    }
}

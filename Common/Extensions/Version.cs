using System;

namespace Imagin.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class VersionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Maximum"></param>
        /// <param name="Minimum"></param>
        /// <returns></returns>
        public static Version Coerce(this Version Value, Version Maximum, Version Minimum)
        {
            return Value > Maximum ? Maximum : (Value < Minimum ? Minimum : Value);
        }
    }
}

using System;

namespace Imagin.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Checks if specified object's type is equal to specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool Equals<T>(this Type Value)
        {
            return Value == typeof(T);
        }

        /// <summary>
        /// Checks if given type implements interface (T).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool Implements<T>(this Type Value)
        {
            return typeof(T).IsAssignableFrom(Value);
        }
    }
}

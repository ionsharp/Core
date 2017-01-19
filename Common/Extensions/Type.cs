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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Type"></param>
        /// <returns></returns>
        public static bool IsAssignableFrom<T>(this Type Type)
        {
            return Type.IsAssignableFrom(typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static T TryCreate<T>(this Type Value)
        {
            try
            {
                return (T)Activator.CreateInstance(Value);
            }
            catch
            {
                return default(T);
            }
        }
    }
}

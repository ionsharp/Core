using System;

namespace Imagin.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets whether or not the type is equal to type, <see cref="{TType}"/>.
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool Equals<TType>(this Type Value)
        {
            return Value == typeof(TType);
        }

        /// <summary>
        /// Gets whether or not the type implements interface, <see cref="{TType}"/> (or whether <see cref="{TType}"/> is assignable from the type).
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool Implements<TType>(this Type Value) where TType : class
        {
            if (!typeof(TType).IsInterface)
                throw new InvalidCastException("Type is not an interface.");

            return typeof(TType).IsAssignableFrom(Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool IsNullable(this Type Value)
        {
            if (!Value.IsGenericType)
                return false;

            return Value.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Attempts to create a new instance of given type using <see cref="Activator.CreateInstance()"/>.
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
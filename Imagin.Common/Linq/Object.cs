using System;
using System.ComponentModel;
using System.Reflection;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class ObjectExtensions
    {
        /// <summary>
        /// Returns object as specified type.
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Type As<Type>(this object source) => source is Type ? (Type)source : default(Type);

        /// <summary>
        /// Check if object is equal to all given objects.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool EqualsAll(this object source, params object[] values)
        {
            foreach (var i in values)
            {
                if (source != i)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Check if object is equal to no given object.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool EqualsNone(this object source, params object[] values)
        {
            foreach (var i in values)
            {
                if (source == i)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Get value for object from given property name.
        /// </summary>
        /// <param name="source">The object to get the value for.</param>
        /// <param name="propertyName">The name of the property to get a value for.</param>
        /// <returns>The value of the property for an object.</returns>
        public static object GetValue(this object source, string propertyName) => source.GetType().GetProperty(propertyName).GetValue(source, null);

        /// <summary>
        /// Checks if given object's type implements interface (T).
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool Implements<TType>(this object source) where TType : class => (bool)source.GetType()?.GetTypeInfo()?.Implements<TType>();

        /// <summary>
        /// Checks if specified object is of specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool Is<T>(this object source) => source is T;

        /// <summary>
        /// Checks if specified object is of specified type.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static bool IsAny(this object source, params Type[] types)
        {
            foreach (var i in types)
            {
                var Type = source.GetType();
                if (Type == i || Type.GetTypeInfo().IsSubclassOf(i))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if specified object is NOT of specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNot<T>(this object value) => !value.Is<T>();

        /// <summary>
        /// Checks if specified object is null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNull(this object value) => value == null;

        /// <summary>
        /// Gets whether or not the <see cref="object"/> is <see cref="Nullable"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullable(this object value) => value.IsNullable<object>();

        /// <summary>
        /// Cast object to given type.
        /// </summary>
        /// <typeparam name="TCast"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TCast To<TCast>(this object value) => (TCast)value;
    }
}

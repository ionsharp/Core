using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns object as specified type.
        /// </summary>
        public static T As<T>(this object Value)
        {
            if (Value is T)
                return (T)Value;
            return default(T);
        }

        /// <summary>
        /// Check if object is equal to all given objects.
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Values"></param>
        /// <returns></returns>
        public static bool EqualsAll(this object Value, params object[] Values)
        {
            foreach (var i in Values)
            {
                if (Value != i)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Check if object is equal to any given object.
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Values"></param>
        /// <returns></returns>
        public static bool EqualsAny<T>(this T Value, params T[] Values)
        {
            foreach (var i in Values)
            {
                if (Value.Equals(i))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Check if object is equal to no given object.
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Values"></param>
        /// <returns></returns>
        public static bool EqualsNone(this object Value, params object[] Values)
        {
            foreach (var i in Values)
            {
                if (Value == i)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Get value for object from given property name.
        /// </summary>
        /// <param name="Value">The object to get the value for.</param>
        /// <param name="PropertyName">The name of the property to get a value for.</param>
        /// <returns>The value of the property for an object.</returns>
        public static object GetValue(this object Value, string PropertyName)
        {
            return Value.GetType().GetProperty(PropertyName).GetValue(Value, null);
        }

        /// <summary>
        /// Checks if given object's type implements interface (T).
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool Implements<TType>(this object Value) where TType : class
        {
            return (bool)Value.GetType()?.GetTypeInfo()?.Implements<TType>();
        }

        /// <summary>
        /// Checks if specified object is of specified type.
        /// </summary>
        public static bool Is<T>(this object ToEvaluate)
        {
            return ToEvaluate is T;
        }

        /// <summary>
        /// Checks if specified object is of specified type.
        /// </summary>
        public static bool IsAny(this object ToEvaluate, params Type[] Types)
        {
            foreach (var i in Types)
            {
                var Type = ToEvaluate.GetType();
                if (Type == i || Type.GetTypeInfo().IsSubclassOf(i))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if specified object is NOT of specified type.
        /// </summary>
        public static bool IsNot<T>(this object Value)
        {
            return !Value.Is<T>();
        }

        /// <summary>
        /// Checks if specified object is null.
        /// </summary>
        public static bool IsNull(this object Value)
        {
            return Value == null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool IsNullable(this object Value)
        {
            return Value.IsNullable<object>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool IsNullable<T>(this T Value)
        {
            return Value.GetType().IsNullable();
        }

        /// <summary>
        /// Return object if it is null or given object.
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Or"></param>
        /// <returns></returns>
        public static object NullOr(this object Value, object Or)
        {
            return Value == null ? null : Or;
        }

        /// <summary>
        /// Return object if it is null or given object.
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Or"></param>
        /// <returns></returns>
        public static T NullOr<T>(this object Value, object Or)
        {
            return Value == null ? default(T) : (T)Or;
        }

        /// <summary>
        /// Cast object to given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static T To<T>(this object Value)
        {
            return (T)Value;
        }
    }
}

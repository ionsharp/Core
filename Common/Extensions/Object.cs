using System;
using System.Linq;
using System.Collections.Generic;

namespace Imagin.Common.Extensions
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
        /// <param name="ToEvaluateAgainst"></param>
        /// <returns></returns>
        public static bool EqualsAll(this object Value, params object[] ToEvaluateAgainst)
        {
            foreach (object i in ToEvaluateAgainst)
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
        /// <param name="ToEvaluateAgainst"></param>
        /// <returns></returns>
        public static bool EqualsAny(this object Value, params object[] ToEvaluateAgainst)
        {
            foreach (var i in ToEvaluateAgainst)
            {
                if (Value == i)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Check if object is equal to no given object.
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="ToEvaluateAgainst"></param>
        /// <returns></returns>
        public static bool EqualsNone(this object Value, params object[] ToEvaluateAgainst)
        {
            foreach (object i in ToEvaluateAgainst)
            {
                if (Value == i)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Get attribute for member of specified object and of specified type.
        /// </summary>
        /// <typeparam name="T">The type of attribute to get.</typeparam>
        /// <param name="ToEvaluate">The object containing the target attribute.</param>
        /// <param name="Member">The name of the member to get the attribute from; if object is enum, field attribute is obtained.</param>
        /// <param name="Inherits">Whether or not to check inherited attributes.</param>
        /// <returns>Attribute for member of object and specified type.</returns>
        public static T GetAttribute<T>(this object ToEvaluate, string Member = "", bool Inherits = false) where T : Attribute
        {
            if (ToEvaluate != null)
            {
                var Items = ToEvaluate.GetAttributes<T>(Member, Inherits);
                return Items.Count() > 0 ? Items.First() : default(T);
            }
            return default(T);
        }

        /// <summary>
        /// Get all attributes for member of specified object and of specified type.
        /// </summary>
        /// <typeparam name="T">The type of attributes to get.</typeparam>
        /// <param name="ToEvaluate">The object containing the target attributes.</param>
        /// <param name="Member">The name of the member to get attributes from; if object is enum, field attributes are obtained.</param>
        /// <param name="Inherits">Whether or not to check inherited attributes.</param>
        /// <returns>Attributes for member of object and specified type.</returns>
        public static IEnumerable<T> GetAttributes<T>(this object ToEvaluate, string Member = "", bool Inherits = false) where T : Attribute
        {
            if (ToEvaluate != null)
                return ToEvaluate.GetAttributes(Member, Inherits).Where(x => x is T).Cast<T>();
            return Enumerable.Empty<T>();
        }

        /// <summary>
        /// Get all attributes for member of specified object.
        /// </summary>
        /// <param name="ToEvaluate">The object containing the target attributes.</param>
        /// <param name="Member">The name of the member to get attributes from; if object is enum, field attributes are obtained.</param>
        /// <param name="Inherits">Whether or not to check inherited attributes.</param>
        /// <returns>Attributes for member of object.</returns>
        public static IEnumerable<object> GetAttributes(this object ToEvaluate, string Member = "", bool Inherits = false)
        {
            if (ToEvaluate != null)
            {
                var Type = ToEvaluate.GetType();
                var Info = Type.GetMember(ToEvaluate.GetType().IsEnum ? ToEvaluate.ToString() : Member);
                if (Info != null && Info.Length >= 1)
                    return Info[0].GetCustomAttributes(Inherits);
            }
            return Enumerable.Empty<object>();
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
        /// Check if member of specified object has attribute of specified type.
        /// </summary>
        /// <typeparam name="T">The type of attribute to check exists.</typeparam>
        /// <param name="ToEvaluate">The object containing the target attribute.</param>
        /// <param name="Member"></param>
        /// <param name="Inherits">Whether or not to check inherited attributes.</param>
        /// <returns>Whether or not the member of the object (or the object itself) has the attribute.</returns>
        public static bool HasAttribute<T>(this object ToEvaluate, string Member = "", bool Inherits = false) where T : Attribute
        {
            if (ToEvaluate != null)
            {
                var Type = ToEvaluate.GetType();
                var Info = Type.GetMember(ToEvaluate.GetType().IsEnum ? ToEvaluate.ToString() : Member);
                if (Info != null && Info.Length >= 1)
                    return Info[0].GetCustomAttributes(typeof(T), Inherits).Any(x => (Inherits ? x.Is<T>() : x.GetType().Equals<T>()));
            }
            return false;
        }

        /// <summary>
        /// Check if specified object has property with specified name.
        /// </summary>
        /// <param name="ToEvaluate">The object to evaluate.</param>
        /// <param name="PropertyName">The name of the property to check exists.</param>
        /// <returns>Whether or not the specified object has a property with the specified name.</returns>
        public static bool HasProperty(this object ToEvaluate, string PropertyName)
        {
            return ToEvaluate.GetType().GetProperty(PropertyName) != null;
        }

        /// <summary>
        /// Checks if given object's type implements interface (T).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool Implements<T>(this object Value)
        {
            return Value.GetType().Implements<T>();
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
                if (Type == i || Type.IsSubclassOf(i))
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
        /// Checks if object has given property.
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="PropertyName"></param>
        /// <returns></returns>
        [Obsolete("Should use identical HasProperty extension method instead; this extension method will be removed in a future version.")]
        public static bool PropertyExists(this object Value, string PropertyName)
        {
            return Value.GetType().GetProperty(PropertyName) != null;
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

        /// <summary>
        /// Convert object to dynamic type.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static dynamic ToDynamic(this object Value)
        {
            return (dynamic)Value;
        }
    }
}

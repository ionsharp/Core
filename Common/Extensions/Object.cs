using System;
using System.Linq;
using System.Collections.Generic;

namespace Imagin.Common.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns object as specified type.
        /// </summary>
        public static T As<T>(this object ToCast)
        {
            if (ToCast is T)
                return (T)ToCast;
            return default(T);
        }

        public static bool EqualsAll(this object ToEvaluate, params object[] ToEvaluateAgainst)
        {
            foreach (object i in ToEvaluateAgainst)
            {
                if (ToEvaluate != i)
                    return false;
            }
            return true;
        }

        public static bool EqualsAny(this object ToEvaluate, params object[] ToEvaluateAgainst)
        {
            foreach (var i in ToEvaluateAgainst)
            {
                if (ToEvaluate == i)
                    return true;
            }
            return false;
        }

        public static bool EqualsNone(this object ToEvaluate, params object[] ToEvaluateAgainst)
        {
            foreach (object i in ToEvaluateAgainst)
            {
                if (ToEvaluate == i)
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
        /// Check if member of specified object has attribute of specified type.
        /// </summary>
        /// <typeparam name="T">The type of attribute to check exists.</typeparam>
        /// <param name="ToEvaluate">The object containing the target attribute.</param>
        /// <param name="Inherits">Whether or not to check inherited attributes.</param>
        /// <returns>Whether or not the member of the object (or the object itself) has the attribute.</returns>
        public static bool HasAttribute<T>(this object ToEvaluate, string Member = "", bool Inherits = false) where T : Attribute
        {
            if (ToEvaluate != null)
            {
                var Type = ToEvaluate.GetType();
                var Info = Type.GetMember(ToEvaluate.GetType().IsEnum ? ToEvaluate.ToString() : Member);
                if (Info != null && Info.Length >= 1)
                    return Info[0].GetCustomAttributes(typeof(T), Inherits).Any(x => (Inherits ? x.Is<T>() : x.TypeEquals<T>()));
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
        /// Checks if specified object is null.
        /// </summary>
        public static bool IsNull(this object ToEvaluate)
        {
            return ToEvaluate == null;
        }

        [Obsolete("Should use identical HasProperty extension method instead; this extension method will be removed in a future version.")]
        public static bool PropertyExists(this object ToEvaluate, string PropertyName)
        {
            return ToEvaluate.GetType().GetProperty(PropertyName) != null;
        }

        public static dynamic ToDynamic(this object ToConvert)
        {
            return (dynamic)ToConvert;
        }

        /// <summary>
        /// Checks if specified object's type is equal to specified type.
        /// </summary>
        public static bool TypeEquals<T>(this object ToEvaluate)
        {
            return ToEvaluate.GetType() == typeof(T);
        }
    }
}

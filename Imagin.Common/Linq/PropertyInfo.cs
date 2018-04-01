using System;
using System.Collections.Generic;
using System.Reflection;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class MemberInfoExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TAttribute GetAttribute<TAttribute>(this MemberInfo value) where TAttribute : Attribute
        {
            foreach (var i in value.GetCustomAttributes(true))
            {
                if (i is TAttribute)
                    return (TAttribute)i;
            }
            return default(TAttribute);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<TAttribute> GetAttributes<TAttribute>(this MemberInfo value) where TAttribute : Attribute
        {
            foreach (var i in value.GetCustomAttributes(true))
                yield return (TAttribute)i;

            yield break;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<Attribute> GetAttributes(this MemberInfo value)
        {
            foreach (var i in value.GetCustomAttributes(true))
                yield return (Attribute)i;

            yield break;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HasAttribute<TAttribute>(this MemberInfo value)
        {
            foreach (var i in value.GetCustomAttributes(true))
            {
                if (i is TAttribute)
                    return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// Check whether or not property is public and can be written to.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool IsPublic(this PropertyInfo Value)
        {
            return Value.CanWrite && Value.GetSetMethod(true).IsPublic;
        }
    }
}

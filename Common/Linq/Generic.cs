using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq;

namespace Imagin.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class GenericExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> Value)
        {
            return Value ?? Enumerable.Empty<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Action"></param>
        public static void ForEach<T>(this IEnumerable<T> Source, Action<T> Action)
        {
            foreach (var i in Source)
                Action(i);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Items"></param>
        /// <returns></returns>
        public static bool In<T>(this T Source, params T[] Items)
        {
            if (null == Source)
                throw new ArgumentNullException("source");
            return Items.Contains(Source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static T[] Merge<T>(this T[] a, T[] b)
        {
            var al = a.Length;
            var bl = b.Length;
            Array.Resize<T>(ref a, al + bl);
            Array.Copy(b, 0, a, al, bl);

            return a;
        }

        /// <summary>
        /// Throws an ArgumentNullException if the given data item is null.
        /// </summary>
        /// <param name="Object">The item to check for nullity.</param>
        /// <param name="Name">The name to use when throwing an exception, if necessary</param>
        public static void ThrowIfNull<T>(this T Object, string Name = "") where T : class
        {
            if (Object == null)
                throw Name.IsNullOrEmpty() ? new ArgumentNullException() : new ArgumentNullException(Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Items"></param>
        /// <param name="Predicate"></param>
        /// <returns></returns>
        public static T WhereFirst<T>(this IEnumerable<T> Items, Func<T, bool> Predicate)
        {
            var Found = Items.Where(Predicate);
            return Found.Any() ? Found.First() : default(T);
        }
    }
}

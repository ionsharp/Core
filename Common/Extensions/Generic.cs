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
        public static void Add<T>(this T[] Value, T Item)
        {
            var i = Value.Length;
            Array.Resize(ref Value, i + 1);
            Value[i - 1] = Item;
        }

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

        public static void For<T>(this IList<T> Source, int From, Action<IList<T>, int> Action)
        {
            for (var i = From; i < Source.Count(); i++)
                Action(Source, i);
        }

        public static void For<T>(this IList<T> Source, int From, int Until, Action<IList<T>, int> Action)
        {
            for (var i = From; i < Until; i++)
                Action(Source, i);
        }

        public static void For<T>(this IList<T> Source, int From, Predicate<int> Until, Action<IList<T>, int> Action)
        {
            for (var i = From; Until(i); i++)
                Action(Source, i);
        }

        public static void For<T>(this IEnumerable<T> Source, int From, Action<IEnumerable<T>, int> Action)
        {
            for (var i = From; i < Source.Count(); i++)
                Action(Source, i);
        }

        public static void For<T>(this IEnumerable<T> Source, int From, int Until, Action<IEnumerable<T>, int> Action)
        {
            for (var i = From; i < Until; i++)
                Action(Source, i);
        }

        public static void For<T>(this IEnumerable<T> Source, int From, Predicate<int> Until, Action<IEnumerable<T>, int> Action)
        {
            for (var i = From; Until(i); i++)
                Action(Source, i);
        }

        /// <summary>
        /// Perform for each loop on given <see cref="IEnumerable{T}"/>.
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

        public static bool TryAdd<T>(this IList<T> Items, T Item)
        {
            try
            {
                Items.Add(Item);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TryClear<T>(this IList<T> Items)
        {
            try
            {
                Items.Clear();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Attempt to perform for each loop on given <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Action"></param>
        public static bool TryForEach<T>(this IEnumerable<T> Source, Action<T> Action)
        {
            try
            {
                foreach (var i in Source)
                    Action(i);
                return true;
            }
            catch
            {
                return false;
            }
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

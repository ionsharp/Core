using Imagin.Common.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class IListExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <param name="comparer"></param>
        public static void AddSorted<T>(this IList<T> list, T item, IComparer<T> comparer = null)
        {
            if (comparer == null)
                comparer = Comparer<T>.Default;

            int i = 0;
            while (i < list.Count && comparer.Compare(list[i], item) < 0)
                i++;

            list.Insert(i, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        public static bool Any(this IList Source) 
        {
            if (Source != null)
            {
                foreach (var i in Source)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        public static object FirstOrDefault(this IList Source)
        {
            var result = default(object);

            if (Source == null)
                return result;

            foreach (var i in Source)
                return i;

            return result;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Action"></param>
        public static void ForEach(this IList Source, Action<object> Action)
        {
            foreach (var i in Source)
                Action(i);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        public static object Last(this IList Source)
        {
            var Result = default(object);
            Source.ForEach(i => Result = i);
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Selector"></param>
        /// <param name="Direction"></param>
        public static void Sort<TSource, TKey>(this IList<TSource> Source, Func<TSource, TKey> Selector, SortDirection Direction)
        {
            var Sorted = default(List<TSource>);
            if (Direction == SortDirection.Ascending)
            {
                Sorted = Source.OrderBy(Selector).ToList();
            }
            else Sorted = Source.OrderByDescending(Selector).ToList();

            Source.Clear();
            foreach (var i in Sorted)
                Source.Add(i);
        }
    }
}

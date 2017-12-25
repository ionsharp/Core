using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class IListGenericExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="Source"></param>
        /// <returns></returns>
        public static TItem First<TItem>(this IList<TItem> Source)
        {
            return Source.Any() ? Source[0] : default(TItem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Source"></param>
        /// <param name="From"></param>
        /// <param name="Action"></param>
        public static void For<T>(this IList<T> Source, int From, Action<IList<T>, int> Action)
        {
            for (var i = From; i < Source.Count(); i++)
                Action(Source, i);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Source"></param>
        /// <param name="From"></param>
        /// <param name="Until"></param>
        /// <param name="Action"></param>
        public static void For<T>(this IList<T> Source, int From, int Until, Action<IList<T>, int> Action)
        {
            for (var i = From; i < Until; i++)
                Action(Source, i);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Source"></param>
        /// <param name="From"></param>
        /// <param name="Until"></param>
        /// <param name="Action"></param>
        public static void For<T>(this IList<T> Source, int From, Predicate<int> Until, Action<IList<T>, int> Action)
        {
            for (var i = From; Until(i); i++)
                Action(Source, i);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="Source"></param>
        /// <returns></returns>
        public static TItem Last<TItem>(this IList<TItem> Source)
        {
            return Source.Any() ? Source[Source.Count - 1] : default(TItem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Value"></param>
        /// <param name="Where"></param>
        public static void Remove<T>(this IList<T> Value, Predicate<T> Where)
        {
            for (var i = Value.Count - 1; i >= 0; i--)
            {
                if (Where(Value[i]))
                    Value.Remove(Value[i]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Items"></param>
        /// <param name="Item"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Items"></param>
        /// <returns></returns>
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
    }
}

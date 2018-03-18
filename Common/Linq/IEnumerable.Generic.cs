using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class IEnumerableExtensions
    {
        static TValue Compare<TValue>(this IEnumerable<TValue> source, Func<TValue, TValue, bool> action, TValue origin = default(TValue))
        {
            var result = origin;
            foreach (var i in source)
            {
                if (action(i, result))
                    result = i;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T At<T>(this IEnumerable<T> source, int index)
        {
            var j = 0;
            foreach (var i in source)
            {
                if (j == index)
                    return i;

                j++;
            }
            return default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool Contains<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            foreach (var i in source)
            {
                if (predicate(i))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Source"></param>
        /// <param name="From"></param>
        /// <param name="Action"></param>
        public static void For<T>(this IEnumerable<T> Source, int From, Action<IEnumerable<T>, int> Action)
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
        public static void For<T>(this IEnumerable<T> Source, int From, int Until, Action<IEnumerable<T>, int> Action)
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
        /// <param name="Source"></param>
        /// <returns></returns>
        public static T Last<T>(this IEnumerable<T> Source)
        {
            var Result = default(T);
            Source.ForEach(i => Result = i);
            return Result;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Source"></param>
        /// <returns></returns>
        public static T LeastCommon<T>(this IEnumerable<T> Source)
        {
            return
            (
                from i in Source
                group i by i into g
                orderby g.Count() ascending
                select g.Key
            )
            .First();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Double Maximum(this IEnumerable<Double> source) => source.Compare((i, j) => i > j);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Int16 Maximum(this IEnumerable<Int16> source) => source.Compare((i, j) => i > j);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Int32 Maximum(this IEnumerable<Int32> source) => source.Compare((i, j) => i > j);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Int64 Maximum(this IEnumerable<Int64> source) => source.Compare((i, j) => i > j);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Double Minimum(this IEnumerable<Double> source) => source.Compare((i, j) => i < j, Double.MaxValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Int16 Minimum(this IEnumerable<Int16> source) => source.Compare((i, j) => i < j, Int16.MaxValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Int32 Minimum(this IEnumerable<Int32> source) => source.Compare((i, j) => i < j, Int32.MaxValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Int64 Minimum(this IEnumerable<Int64> source) => source.Compare((i, j) => i < j, Int64.MaxValue);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Source"></param>
        /// <returns></returns>
        public static T MostCommon<T>(this IEnumerable<T> Source)
        {
            new List<int>().Maximum();

            return 
            (
                from i in Source
                group i by i into g
                orderby g.Count() descending
                select g.Key
            )
            .First();
        }

        /// <summary>
        /// Gets the second item in a sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static T Second<T>(this IEnumerable<T> Value)
        {
            var j = 0;
            foreach (var i in Value)
            {
                if (j == 1) return i;
                j++;
            }
            return default(T);
        }

        /// <summary>
        /// Gets the last item in a sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static T Third<T>(this IEnumerable<T> Value)
        {
            var j = 0;
            foreach (var i in Value)
            {
                if (j == 2) return i;
                j++;
            }
            return default(T);
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
    }
}

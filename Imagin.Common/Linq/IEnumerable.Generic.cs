using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imagin.Common.Linq
{
    public static partial class XIEnumerable
    {
        static TValue Compare<TValue>(this IEnumerable<TValue> input, Func<TValue, TValue, bool> action, TValue origin = default)
        {
            var result = origin;
            foreach (var i in input)
            {
                if (action(i, result))
                    result = i;
            }
            return result;
        }

        //...

        public static bool Contains<T>(this IEnumerable<T> input, Predicate<T> predicate)
        {
            foreach (var i in input)
            {
                if (predicate(i))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Perform for each loop on given <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> input, Action<T> action)
        {
            foreach (var i in input)
                action(i);
        }

        public static T Last<T>(this IEnumerable<T> input)
        {
            var Result = default(T);
            input.ForEach(i => Result = i);
            return Result;
        }
        
        public static T LeastCommon<T>(this IEnumerable<T> input)
        {
            return
            (
                from i in input
                group i by i into g
                orderby g.Count() ascending
                select g.Key
            )
            .FirstOrDefault<T>();
        }

        /// <summary>
        /// Returns the maximal element of the given sequence, based on
        /// the given projection.
        /// </summary>
        /// <remarks>
        /// If more than one element has the maximal projected value, the first
        /// one encountered will be returned. This overload uses the default comparer
        /// for the projected type. This operator uses immediate execution, but
        /// only buffers a single result (the current maximal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="input">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <returns>The maximal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="input"/> or <paramref name="selector"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="input"/> is empty</exception>
        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> input, Func<TSource, TKey> selector) => input.MaxBy(selector, null);

        /// <summary>
        /// Returns the maximal element of the given sequence, based on
        /// the given projection and the specified comparer for projected values. 
        /// </summary>
        /// <remarks>
        /// If more than one element has the maximal projected value, the first
        /// one encountered will be returned. This operator uses immediate execution, but
        /// only buffers a single result (the current maximal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="input">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <param name="comparer">Comparer to use to compare projected values</param>
        /// <returns>The maximal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="input"/>, <paramref name="selector"/> 
        /// or <paramref name="comparer"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="input"/> is empty</exception>
        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> input, Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (input == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");
            comparer = comparer ?? Comparer<TKey>.Default;

            using (var sourceIterator = input.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence contains no elements");
                }
                var max = sourceIterator.Current;
                var maxKey = selector(max);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, maxKey) > 0)
                    {
                        max = candidate;
                        maxKey = candidateProjected;
                    }
                }
                return max;
            }
        }

        /// <summary>
        /// Returns the minimal element of the given sequence, based on
        /// the given projection.
        /// </summary>
        /// <remarks>
        /// If more than one element has the minimal projected value, the first
        /// one encountered will be returned. This overload uses the default comparer
        /// for the projected type. This operator uses immediate execution, but
        /// only buffers a single result (the current minimal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="input">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <returns>The minimal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="input"/> or <paramref name="selector"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="input"/> is empty</exception>
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> input, Func<TSource, TKey> selector)
        {
            return input.MinBy(selector, null);
        }

        /// <summary>
        /// Returns the minimal element of the given sequence, based on
        /// the given projection and the specified comparer for projected values.
        /// </summary>
        /// <remarks>
        /// If more than one element has the minimal projected value, the first
        /// one encountered will be returned. This operator uses immediate execution, but
        /// only buffers a single result (the current minimal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="input">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <param name="comparer">Comparer to use to compare projected values</param>
        /// <returns>The minimal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="input"/>, <paramref name="selector"/> 
        /// or <paramref name="comparer"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="input"/> is empty</exception>
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> input, Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (input == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");
            comparer = comparer ?? Comparer<TKey>.Default;

            using (var sourceIterator = input.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence contains no elements");
                }
                var min = sourceIterator.Current;
                var minKey = selector(min);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, minKey) < 0)
                    {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }
                return min;
            }
        }

        public static T MostCommon<T>(this IEnumerable<T> input)
        {
            return 
            (
                from i in input
                group i by i into g
                orderby g.Count() descending
                select g.Key
            )
            .FirstOrDefault<T>();
        }

        public static string ToString<T>(this IEnumerable<T> input, string separator, Func<T, string> format = null)
        {
            var result = new StringBuilder();

            var j = 0;
            var k = input.Count() - 1;
            foreach (var i in input)
            {
                result.Append(format?.Invoke(i) ?? $"{i}");
                if (j < k)
                    result.Append(separator);

                j++;
            }
            return result.ToString();
        }
    }
}

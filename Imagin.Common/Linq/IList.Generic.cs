using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagin.Common.Linq
{
    public static partial class XIList
    {
        public static void AddSorted<T>(this IList<T> input, T item, IComparer<T> comparer = null)
        {
            if (comparer == null)
                comparer = Comparer<T>.Default;

            int i = 0;
            while (i < input.Count && comparer.Compare(input[i], item) < 0)
                i++;

            input.Insert(i, item);
        }

        public static T First<T>(this IList<T> input) => input.Any() ? input[0] : default;

        public static void For<T>(this IList<T> input, int from, Action<IList<T>, int> action)
        {
            for (var i = from; i < input.Count(); i++)
                action(input, i);
        }

        public static void For<T>(this IList<T> input, int from, int until, Action<IList<T>, int> action)
        {
            for (var i = from; i < until; i++)
                action(input, i);
        }

        public static void For<T>(this IList<T> input, int from, Predicate<int> until, Action<IList<T>, int> action)
        {
            for (var i = from; until(i); i++)
                action(input, i);
        }

        public static int IndexOf<T>(this IList<T> input, Predicate<T> where)
        {
            var result = 0;
            foreach (var i in input)
            {
                if (i is T j)
                {
                    if (where(j))
                        return result;
                }
                result++;
            }
            return -1;
        }

        public static T Last<T>(this IList<T> input) => input.Any() ? input[input.Count - 1] : default;

        public static void Remove<T>(this IList<T> input, Predicate<T> where)
        {
            for (var i = input.Count - 1; i >= 0; i--)
            {
                if (where(input[i]))
                    input.Remove(input[i]);
            }
        }

        public static void Shuffle<T>(this IList<T> input)
        {
            int n = input.Count;
            while (n > 1)
            {
                n--;
                int k = Random.NextInt32(n + 1);
                T value = input[k];
                input[k] = input[n];
                input[n] = value;
            }
        }
    }
}
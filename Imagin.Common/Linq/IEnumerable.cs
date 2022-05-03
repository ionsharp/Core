using System;
using System.Collections;
using System.Collections.Generic;

namespace Imagin.Common.Linq
{
    public static partial class XIEnumerable
    {
        public static bool Empty(this IEnumerable input)
        {
            foreach (var i in input)
                return false;

            return true;
        }

        public static void ForEach(this IEnumerable input, Action<object> action)
        {
            foreach (var i in input)
                action(i);
        }

        public static object First(this IEnumerable input)
        {
            foreach (var i in input)
                return i;

            return null;
        }

        public static T FirstOrDefault<T>(this IEnumerable input)
        {
            if (input != null)
            {
                foreach (var i in input)
                {
                    if (i is T j)
                        return j;
                }
            }
            return default;
        }

        public static object Last(this IEnumerable input)
        {
            object result = default;
            input.ForEach(i => result = i);
            return result;
        }

        public static IEnumerable<T> Select<T>(this IEnumerable input, Func<object, T> action)
        {
            foreach (var i in input)
                yield return action(i);
        }
    }
}
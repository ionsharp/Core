using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imagin.Core.Linq
{
    public static partial class XList
    {
        public static void Add(this IList input, IEnumerable items)
        {
            foreach (var i in items)
                input.Add(i);
        }

        /// <summary>
        /// Adds the given item if it doesn't yet exist (see <see cref="IList.Contains(object)"/>).
        /// </summary>
        public static void AddOnce<T>(this IList input, T item)
        {
            if (!input.Contains(item))
                input.Add(item);
        }

        public static bool Any(this IList input)
        {
            if (input != null)
            {
                foreach (var i in input)
                    return true;
            }
            return false;
        }

        public static bool Contains<T>(this IList input) => input.Contains<object>(i => i is T);

        public static bool Contains(this IList input, Predicate<object> predicate) => input.Contains<object>(i => predicate(i));

        public static bool Contains<T>(this IList input, Predicate<T> predicate)
        {
            foreach (var i in input)
            {
                if (i is T j)
                {
                    if (predicate(j))
                        return true;
                }
            }
            return false;
        }

        public static int Count<T>(this IList input, Predicate<T> predicate) => input.Where(predicate).Count();

        public static T First<T>(this IList input)
        {
            foreach (var i in input)
            {
                if (i is T j)
                    return j;
            }
            throw new InvalidOperationException("Sequence contains no elements.");
        }

        public static object FirstOrDefault(this IList input)
        {
            object result = null;

            if (input == null)
                return result;

            foreach (var i in input)
                return i;

            return result;
        }

        public static T FirstOrDefault<T>(this IList input, Predicate<T> predicate = null)
        {
            foreach (var i in input)
            {
                if (i is T j && predicate?.Invoke(j) != false)
                    return j;
            }
            return default;
        }

        public static void ForEach(this IList input, Action<object> Action)
        {
            foreach (var i in input)
                Action(i);
        }

        public static void ForEach<T>(this IList input, Action<T> action)
        {
            foreach (var i in input)
            {
                if (i is T j)
                    action(j);
            }
        }

        public static int IndexOf<T>(this IList input, Predicate<T> predicate)
        {
            var index = 0;
            foreach (var i in input)
            {
                if (i is T j)
                {
                    if (predicate(j))
                        return index;
                }
                index++;
            }
            return -1;
        }

        public static object Last(this IList input)
        {
            object result = default;
            input.ForEach(i => result = i);
            return result;
        }

        public static T Last<T>(this IList input, int index)
        {
            if (input is IList<T> i)
            {
                if (i.Count > 0)
                {
                    var lastIndex = input.Count - 1;
                    var actualIndex = (lastIndex - index).Clamp(lastIndex, 0);
                    return i[actualIndex];
                }
            }
            throw new InvalidCastException();
        }

        public static object LastOrDefault(this IList input, int index)
        {
            if (input?.Count > 0)
            {
                var lastIndex = input.Count - 1;
                var actualIndex = (lastIndex - index).Clamp(lastIndex, 0);
                return input[actualIndex];
            }
            return default;
        }

        public static void MoveDown(this IList input, int index, bool wrap)
        {
            var i = input[index];
            if (index + 1 <= input.Count - 2)
            {
                input.RemoveAt(index);
                input.Insert(index + 1, i);
            }
            else if (index + 1 <= input.Count - 1)
            {
                input.RemoveAt(index);
                input.Add(i);
            }
            else if (wrap)
            {
                input.RemoveAt(index);
                input.Insert(0, i);
            }
        }

        public static void MoveUp(this IList input, int index, bool wrap)
        {
            if (index - 1 >= 0)
            {
                var i = input[index - 1];
                input.RemoveAt(index - 1);
                input.Insert(index, i);
            }
            else if (wrap)
            {
                var j = input[index];
                input.RemoveAt(index);
                input.Add(j);
            }
        }

        public static void RemoveLast(this IList input) => input.RemoveAt(input.Count - 1);

        public static void RemoveWhere(this IList input, Predicate<object> where)
        {
            for (var i = input.Count - 1; i >= 0; i--)
            {
                if (where(input[i]))
                    input.RemoveAt(i);
            }
        }

        public static IEnumerable<T> Select<T>(this IList input, Func<object, T> select)
        {
            foreach (var i in input)
                yield return select(i);
        }

        public static IEnumerable<T2> Select<T1, T2>(this IList input, Func<T1, T2> selector)
        {
            foreach (var i in input)
            {
                if (i is T1 t1)
                    yield return selector(t1);
            }
            yield break;
        }

        public static object[] ToArray(this IList input) => input.Cast<object>().ToArray();

        public static List<object> ToList(this IList input)
        {
            var result = new List<object>();
            input.ForEach(i => result.Add(i));
            return result;
        }

        public static string ToString(this IList input, string separator, Func<object, string> format = null) => input.Select(i => i).ToString(separator, format);

        public static IEnumerable<T> Where<T>(this IList input, Predicate<T> predicate)
        {
            foreach (var i in input)
            {
                if (i is T t)
                {
                    if (predicate(t))
                        yield return t;
                }
            }
        }
    }
}
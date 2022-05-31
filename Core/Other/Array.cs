using System.Collections.Generic;
using System.Linq;

namespace Imagin.Core
{
    public static class Array<T>
    {
        public static T[] Empty => new T[0];

        public static T[] New(params T[] input) => input ?? new T[0];

        public static T[] New(IEnumerable<T> input)
        {
            var count = input?.Count() ?? 0;
            var result = new T[count];

            if (count == 0)
                return result;

            for (var i = 0; i < count; i++)
                result[i] = input.ElementAtOrDefault(i);

            return result;
        }
    }
}
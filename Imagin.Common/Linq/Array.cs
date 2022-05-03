using System;
using System.Collections.Generic;

namespace Imagin.Common.Linq
{
    public static partial class XArray
    {
        public static IEnumerable<object> Where(this Array value, Predicate<object> predicate)
        {
            foreach (var i in value)
            {
                if (predicate(i))
                    yield return i;
            }
            yield break;
        }

        public static string SplitWith(this Array input, string separator)
        {
            var result = string.Empty;

            var k = 0;
            foreach (var j in input)
            {
                result += $"{j}";

                if (k < input.Length - 1)
                    result += $"{separator}";

                k++;
            }

            return result;
        }
    }
}
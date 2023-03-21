using System;
using System.Collections;
using System.Collections.Generic;

namespace Imagin.Core.Linq;

public static class XDictionary
{
    public static Z FirstOrDefault<X, Y, Z>(this Dictionary<X, Y> input, Predicate<KeyValuePair<X, Y>> where = null)
    {
        foreach (var i in input)
        {
            if (i.Value is Z result)
            {
                if (where == null || where.Invoke(i))
                    return result;
            }
        }
        return default;
    }

    public static object GetOrAdd(this IDictionary input, object key, Func<object> defaultValue = null)
    {
        if (key != null)
        {
            if (!input.Contains(key))
                input.Add(key, defaultValue?.Invoke());

            return input[key];
        }
        return null;
    }

    public static void SetOrAdd<T>(this IDictionary input, object key, T value)
    {
        if (key != null)
        {
            if (!input.Contains(key))
                input.Add(key, value);

            input[key] = value;
        }
    }
}
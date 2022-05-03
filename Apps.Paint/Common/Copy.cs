using System;
using System.Collections.Generic;

namespace Imagin.Apps.Paint
{
    /// <summary>
    /// A singleton "clipboard" that stores a single reference for each unique type.
    /// </summary>
    /// <remarks>Instances can only be replaced once added.</remarks>
    public class Copy
    {
        static readonly Dictionary<Type, object> data = new();

        public static bool Contains<T>()
            => data.ContainsKey(typeof(T));

        public static T Get<T>()
            => Contains<T>() ? (T)data[typeof(T)] : default;

        public static void Set<T>(T input)
        {
            if (Contains<T>())
                data[typeof(T)] = input;

            else data.Add(typeof(T), input);
        }
    }
}
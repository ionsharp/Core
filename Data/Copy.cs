using System;
using System.Collections.Generic;

namespace Imagin.Core.Data;

public static class XClipboard
{
    static Dictionary<Type, object> data = new();

    public static bool Contains(Type type) => type != null && data.ContainsKey(type);

    public static void Copy(object input)
    {
        if (input != null)
        {
            var type = input.GetType();
            if (data.ContainsKey(type))
                data[type] = input;

            else data.Add(type, input);
        }
    }

    public static object Paste(Type type) => data.ContainsKey(type) ? data[type] : null;
}
using Imagin.Core.Linq;
using System;
using System.Collections.Generic;

namespace Imagin.Core;

/// <summary>Stores a single-instance <see langword="object"/> with an associated (unique) <see cref="Type"/> for global (indefinite) access.</summary>
public static class Current
{
    static readonly Dictionary<Type, object> instances = new();

    ///

    public static void Add<T>(T instance)
    {
        var type = instance.GetType();
        if (instances.ContainsKey(type))
        {
            instances[type] = instance;
            return;
        }
        instances.Add(type, instance);
    }

    public static T Create<T>(params object[] parameters)
    {
        var result = typeof(T).Create<T>(parameters);
        Add(result);
        return result;
    }

    public static T Get<T>()
    {
        if (instances.ContainsKey(typeof(T)))
            return (T)instances[typeof(T)];

        return instances.FirstOrDefault<Type, object, T>(i => i.Key.Inherits(typeof(T), true) || (typeof(T).IsInterface && i.Key.Implements<T>()));
    }
}
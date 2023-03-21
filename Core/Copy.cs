using Imagin.Core.Collections.ObjectModel;
using Imagin.Core.Linq;
using System;
using System.Linq;

namespace Imagin.Core;

/// <summary>A singleton "clipboard" that stores a single reference for each unique type.</summary>
/// <remarks>Instances can only be replaced once added.</remarks>
public class Copy
{
    public class DataModel : Base<object>
    {
        public Type Type { get => Get<Type>(); set => Set(value); }

        public DataModel() : base() { }

        public DataModel(Type type, object data) : base(data) => Type = type;
    }

    public static ObservableCollection<DataModel> Data { get; private set; } = new();

    ///

    public static bool Contains<T>() => Contains(typeof(T));

    public static bool Contains(Type type) => type != null && Data.Contains(i => i.Type == type);

    ///

    public static T Get<T>() => Get(typeof(T)).As<T>();

    public static object Get(Type type) => Contains(type) ? Data.FirstOrDefault(i => i.Type == type)?.Value : null;

    public static void Set<T>(T input)
    {
        var type = typeof(T);

        if (Contains(type))
        {
            Data.IndexOf(i => i.Type == type).If(i => i >= 0, Data.RemoveAt);
            Data.Add(new(type, input));
        }
        else Data.Add(new(type, input));
    }
}
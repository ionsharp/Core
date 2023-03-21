using System;

namespace Imagin.Core;

public class Accessor<T> : Base
{
    public readonly Func<T> Get;

    public readonly Action<T> Set;

    public T Value
    {
        get => Get();
        set
        {
            Set(value);
            Update(() => Value);
        }
    }

    public Accessor(Func<T> get, Action<T> set)
    {
        Get = get; Set = set;
    }
}

public class BooleanAccessor : Accessor<bool>
{
    public BooleanAccessor(Func<bool> get, Action<bool> set) : base(get, set) { }
}
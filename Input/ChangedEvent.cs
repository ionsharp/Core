using System;

namespace Imagin.Core.Input;

public delegate void ChangedEventHandler<T>(object sender, ChangedEventArgs<T> e);

public class ChangedEventArgs<T> : EventArgs
{
    public readonly T NewValue;

    public readonly T OldValue;

    public ChangedEventArgs(T oldValue, T newValue) : base()
    {
        OldValue = oldValue;
        NewValue = newValue;
    }
}
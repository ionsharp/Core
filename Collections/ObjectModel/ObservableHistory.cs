using Imagin.Core.Linq;
using System;
using System.Collections.Generic;

namespace Imagin.Core.Collections.ObjectModel;

[Serializable]
public class ObservableHistory<T> : ObservableCollection<T>, ILimit
{
    readonly Stack<T> redo = new();

    Limit limit = default;
    public Limit Limit
    {
        get => limit;
        set
        {
            limit = value;
            limit.Coerce(this);
        }
    }

    ///

    T Peek()
    {
        T result = default;
        foreach (var i in this)
            result = i;

        return result;
    }

    T Pop()
    {
        if (Count > 0)
        {
            var result = this.Last<T>();
            Remove(result);
            return result;
        }
        return default;
    }

    ///

    new public void Add(T item)
    {
        if (Peek()?.Equals(item) != true)
        {
            base.Add(item);
            if (!limit.Coerce(this))
                redo.Clear();
        }
    }

    new public void Clear()
    {
        redo.Clear();
        base.Clear();
    }

    ///

    public bool CanRedo() => redo.Count > 0;

    public bool CanUndo() => Count > 0;

    ///

    public bool Redo(Action<T> action)
    {
        if (CanRedo())
        {
            var i = redo.Pop();
            base.Add(i);
            action?.Invoke(i);
            return true;
        }
        return false;
    }

    public bool Undo(Action<T> action)
    {
        if (CanUndo())
        {
            var i = Pop();
            redo.Push(i);
            action?.Invoke(i);
            return true;
        }
        return false;
    }
}

[Serializable]
public class StringHistory : ObservableHistory<string>
{
    public StringHistory(Limit limit = default) : base()
    {
        Limit = limit;
    }
}
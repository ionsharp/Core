using System;
using System.Collections;
using System.Collections.Generic;

namespace Imagin.Core.Collections.Generic;

public abstract class BaseImmutableCollection<T> : ICollection, ICollection<T>, IEnumerable, IEnumerable<T>
{
    /// <summary>
    /// Throws <see cref="NotImplementedException"/>.
    /// </summary>
    bool ICollection.IsSynchronized => throw new NotImplementedException();

    /// <summary>
    /// Throws <see cref="NotImplementedException"/>.
    /// </summary>
    object ICollection.SyncRoot => throw new NotImplementedException();

    public abstract int Count { get; }

    public bool IsReadOnly => true;

    void ICollection.CopyTo(Array Array, int Index) => CopyTo((T[])Array, Index);

    public abstract bool Contains(T item);

    public abstract void CopyTo(T[] Array, int ArrayIndex);

    public abstract IEnumerator<T> GetEnumerator();

    /// <summary>
    /// Throws <see cref="NotSupportedException"/>.
    /// </summary>
    public void Add(T Item) => throw new NotSupportedException();

    /// <summary>
    /// Throws <see cref="NotSupportedException"/>.
    /// </summary>
    public void Clear() => throw (new NotSupportedException());

    /// <summary>
    /// Throws <see cref="NotSupportedException"/>.
    /// </summary>
    public bool Remove(T Item) => throw (new NotSupportedException());

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

using Imagin.Common.Input;
using System;
using System.Collections.Generic;

namespace Imagin.Common.Collections.Generic
{
    /// <summary>
    /// Defines methods to manipulate collections that should "watch" collection changes.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    public interface ITrackableCollection<T>
    {
        event EventHandler<EventArgs<T>> ItemAdded;

        event EventHandler<EventArgs<IEnumerable<T>>> ItemsAdded;

        event EventHandler<EventArgs> ItemsChanged;

        event EventHandler<EventArgs> ItemsCleared;

        event EventHandler<EventArgs<T>> ItemInserted;

        event EventHandler<EventArgs<T>> ItemRemoved;

        event EventHandler<EventArgs<IEnumerable<T>>> ItemsRemoved;

        bool IsEmpty
        {
            get; set;
        }

        int Total
        {
            get; set;
        }

        void Add(T Item);

        void Add(IEnumerable<T> Items);

        void Clear();

        void Insert(int i, T Item);

        bool Remove(T Item);

        void Remove(IEnumerable<T> Items);

        void RemoveAt(int i);

        void RemoveAt(params int[] Indices);
    }
}

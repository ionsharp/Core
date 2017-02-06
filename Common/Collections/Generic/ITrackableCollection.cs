using Imagin.Common.Input;
using System;
using System.Collections.Generic;

namespace Imagin.Common.Collections.Generic
{
    /// <summary>
    /// Specifies a collection capable of tracking generic items.
    /// </summary>
    /// <typeparam name="T">The type of the objects in the collection.</typeparam>
    public interface ITrackableCollection<T>
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EventArgs<T>> ItemAdded;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EventArgs<IEnumerable<T>>> ItemsAdded;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EventArgs> ItemsChanged;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EventArgs> ItemsCleared;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EventArgs<T>> ItemInserted;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EventArgs<T>> ItemRemoved;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EventArgs<IEnumerable<T>>> ItemsRemoved;

        /// <summary>
        /// Gets whether or not the collection is empty.
        /// </summary>
        bool IsEmpty
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        void Add(T Item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Items"></param>
        void Add(IEnumerable<T> Items);

        /// <summary>
        /// 
        /// </summary>
        void Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="Item"></param>
        void Insert(int i, T Item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        void Remove(T Item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Items"></param>
        void Remove(IEnumerable<T> Items);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        void RemoveAt(int i);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Indices"></param>
        void RemoveAt(params int[] Indices);
    }
}

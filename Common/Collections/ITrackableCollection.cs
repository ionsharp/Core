using Imagin.Common.Input;
using System;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
namespace Imagin.Common.Collections
{
    /// <summary>
    /// Specifies a collection capable of tracking items.
    /// </summary>
    public interface ITrackableCollection
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EventArgs<object>> ItemAdded;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EventArgs<IEnumerable<object>>> ItemsAdded;

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
        event EventHandler<EventArgs<object>> ItemInserted;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EventArgs<object>> ItemRemoved;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EventArgs<IEnumerable<object>>> ItemsRemoved;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EventArgs> PreviewItemsCleared;

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
        void Add(object Item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Items"></param>
        void Add(IEnumerable<object> Items);

        /// <summary>
        /// 
        /// </summary>
        void Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="Item"></param>
        void Insert(int i, object Item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        void Remove(object Item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Items"></param>
        void Remove(IEnumerable<object> Items);

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

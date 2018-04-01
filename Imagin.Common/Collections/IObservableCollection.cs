using Imagin.Common.Input;
using System;
using System.Collections.Generic;

namespace Imagin.Common.Collections
{
    /// <summary>
    /// Specifies a collection capable of tracking items.
    /// </summary>
    public interface IObservableCollection
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EventArgs<object>> ItemAdded;

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
        bool Remove(object Item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        void RemoveAt(int i);
    }
}

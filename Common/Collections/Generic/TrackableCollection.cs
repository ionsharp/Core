using Imagin.Common.Collections.Generic;
using Imagin.Common.Extensions;
using Imagin.Common.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Imagin.Common.Collections.ObjectModel
{
    /// <summary>
    /// Provides a collection that can be tracked; inherits <see cref="ObservableCollection{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    public class TrackableCollection<T> : ObservableCollection<T>, ITrackableCollection<T>
    {
        #region Properties

        /// <summary>
        /// Occurs when a single item is added.
        /// </summary>
        public event EventHandler<EventArgs<T>> ItemAdded;

        /// <summary>
        /// Occurs when any number of items are added.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<T>>> ItemsAdded;

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event EventHandler<EventArgs> ItemsChanged;

        /// <summary>
        /// Occurs when the collection is cleared.
        /// </summary>
        public event EventHandler<EventArgs> ItemsCleared;

        /// <summary>
        /// Occurs when a single item is inserted.
        /// </summary>
        public event EventHandler<EventArgs<T>> ItemInserted;

        /// <summary>
        /// Occurs when a single item is removed.
        /// </summary>
        public event EventHandler<EventArgs<T>> ItemRemoved;

        /// <summary>
        /// Occurs when any number of items are removed.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<T>>> ItemsRemoved;

        /// <summary>
        /// 
        /// </summary>
        public new event PropertyChangedEventHandler PropertyChanged;

        bool isEmpty = true;
        /// <summary>
        /// 
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this.isEmpty;
            }
            set
            {
                this.isEmpty = value;
                this.OnPropertyChanged("IsEmpty");
            }
        }

        #endregion

        #region TrackableCollection<T>

        /// <summary>
        /// 
        /// </summary>
        public TrackableCollection() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Items"></param>
        public TrackableCollection(IEnumerable<T> Items) : base(Items)
        {
        }

        #endregion

        #region Methods

        #region Public

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        public new void Add(T Item)
        {
            base.Add(Item);
            OnItemAdded(Item);
            OnItemsChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Items"></param>
        public void Add(IEnumerable<T> Items)
        {
            Items.ForEach(i => base.Add(i));
            OnItemsAdded(Items);
            OnItemsChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        public new void Clear()
        {
            var Items = this;
            base.Clear();
            OnItemsCleared();
            OnItemsChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="Item"></param>
        public new void Insert(int i, T Item)
        {
            base.Insert(i, Item);
            OnItemInserted(Item, i);
            OnItemsChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        public new void Remove(T Item)
        {
            base.Remove(Item);
            OnItemRemoved(Item);
            OnItemsChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Items"></param>
        public void Remove(IEnumerable<T> Items)
        {
            Items.ForEach(i => base.Remove(i));
            OnItemsRemoved(Items);
            OnItemsChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        public new void RemoveAt(int i)
        {
            var Item = this[i];
            base.RemoveAt(i);
            OnItemRemoved(Item);
            OnItemsChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Indices"></param>
        public void RemoveAt(params int[] Indices)
        {
            var Items = new List<T>();
            var j = 0;
            Indices.ForEach(i =>
            {
                Items.Add(this[i]);
                base.Remove(Items[j]);
                j++;
            });
            OnItemsRemoved(Items);
            OnItemsChanged();
        }

        #endregion

        #region Virtual

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        protected virtual void OnItemAdded(T Item)
        {
            ItemAdded?.Invoke(this, new EventArgs<T>(Item));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Items"></param>
        protected virtual void OnItemsAdded(IEnumerable<T> Items)
        {
            ItemsAdded?.Invoke(this, new EventArgs<IEnumerable<T>>(Items));
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnItemsChanged()
        {
            OnPropertyChanged("Count");
            IsEmpty = Count == 0;

            ItemsChanged?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnItemsCleared()
        {
            ItemsCleared?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        /// <param name="Index"></param>
        protected virtual void OnItemInserted(T Item, int Index)
        {
            ItemInserted?.Invoke(this, new EventArgs<T>(Item, Index));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        protected virtual void OnItemRemoved(T Item)
        {
            ItemRemoved?.Invoke(this, new EventArgs<T>(Item));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OldItems"></param>
        protected virtual void OnItemsRemoved(IEnumerable<T> OldItems)
        {
            ItemsRemoved?.Invoke(this, new EventArgs<IEnumerable<T>>(OldItems));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        public virtual void OnPropertyChanged(string Name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));
        }

        #endregion

        #endregion
    }
}

using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Imagin.Common.Collections.Generic
{
    /// <summary>
    /// Provides a collection that can be tracked; inherits <see cref="List{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    public class TrackableList<T> : List<T>, ITrackableCollection, ITrackableCollection<T>
    {
        #region Properties

        event EventHandler<EventArgs<object>> itemAdded;
        event EventHandler<EventArgs<object>> ITrackableCollection.ItemAdded
        {
            add
            {
                itemAdded += value;
            }
            remove
            {
                itemAdded -= value;
            }
        }
        /// <summary>
        /// Occurs when a single item is added.
        /// </summary>
        public event EventHandler<EventArgs<T>> ItemAdded;

        event EventHandler<EventArgs<IEnumerable<object>>> itemsAdded;
        event EventHandler<EventArgs<IEnumerable<object>>> ITrackableCollection.ItemsAdded
        {
            add
            {
                itemsAdded += value;
            }
            remove
            {
                itemsAdded -= value;
            }
        }
        /// <summary>
        /// Occurs when any number of items are added.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<T>>> ItemsAdded;

        event EventHandler<EventArgs> ITrackableCollection.ItemsChanged
        {
            add
            {
                ItemsChanged += value;
            }
            remove
            {
                ItemsChanged -= value;
            }
        }
        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event EventHandler<EventArgs> ItemsChanged;

        event EventHandler<EventArgs> ITrackableCollection.ItemsCleared
        {
            add
            {
                ItemsCleared += value;
            }
            remove
            {
                ItemsCleared -= value;
            }
        }
        /// <summary>
        /// Occurs when the collection is cleared.
        /// </summary>
        public event EventHandler<EventArgs> ItemsCleared;

        event EventHandler<EventArgs<object>> itemInserted;
        event EventHandler<EventArgs<object>> ITrackableCollection.ItemInserted
        {
            add
            {
                itemInserted += value;
            }
            remove
            {
                itemInserted -= value;
            }
        }
        /// <summary>
        /// Occurs when a single item is inserted.
        /// </summary>
        public event EventHandler<EventArgs<T>> ItemInserted;

        event EventHandler<EventArgs<object>> itemRemoved;
        event EventHandler<EventArgs<object>> ITrackableCollection.ItemRemoved
        {
            add
            {
                itemRemoved += value;
            }
            remove
            {
                itemRemoved -= value;
            }
        }
        /// <summary>
        /// Occurs when a single item is removed.
        /// </summary>
        public event EventHandler<EventArgs<T>> ItemRemoved;

        event EventHandler<EventArgs<IEnumerable<object>>> itemsRemoved;
        event EventHandler<EventArgs<IEnumerable<object>>> ITrackableCollection.ItemsRemoved
        {
            add
            {
                itemsRemoved += value;
            }
            remove
            {
                itemsRemoved -= value;
            }
        }
        /// <summary>
        /// Occurs when any number of items are removed.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<T>>> ItemsRemoved;

        event EventHandler<EventArgs> ITrackableCollection.PreviewItemsCleared
        {
            add
            {
                PreviewItemsCleared += value;
            }
            remove
            {
                PreviewItemsCleared -= value;
            }
        }
        /// <summary>
        /// Occurs just before the collection is cleared.
        /// </summary>
        public event EventHandler<EventArgs> PreviewItemsCleared;

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        bool isEmpty = true;
        /// <summary>
        /// 
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return isEmpty;
            }
            private set
            {
                isEmpty = value;
                OnPropertyChanged("IsEmpty");
            }
        }
        bool ITrackableCollection.IsEmpty
        {
            get
            {
                return isEmpty;
            }
        }

        #endregion

        #region TrackableList<T>

        /// <summary>
        /// 
        /// </summary>
        public TrackableList() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Items"></param>
        public TrackableList(IEnumerable<T> Items) : base(Items)
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
        void ITrackableCollection.Add(object Item)
        {
            Add((T)Item);
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
        void ITrackableCollection.Add(IEnumerable<object> Items)
        {
            Add(Items.Cast<T>());
        }

        /// <summary>
        /// 
        /// </summary>
        public new void Clear()
        {
            OnPreviewItemsCleared();
            base.Clear();
            OnItemsCleared();
            OnItemsChanged();
        }
        void ITrackableCollection.Clear()
        {
            Clear();
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
        void ITrackableCollection.Insert(int i, object Item)
        {
            Insert(i, (T)Item);
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
        void ITrackableCollection.Remove(object Item)
        {
            Remove((T)Item);
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
        void ITrackableCollection.Remove(IEnumerable<object> Items)
        {
            Remove(Items.Cast<T>());
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
        void ITrackableCollection.RemoveAt(int i)
        {
            RemoveAt(i);
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
        void ITrackableCollection.RemoveAt(params int[] Indices)
        {
            RemoveAt(Indices);
        }

        #endregion

        #region Virtual

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        protected virtual void OnItemAdded(T Item)
        {
            itemAdded?.Invoke(this, new EventArgs<object>(Item));
            ItemAdded?.Invoke(this, new EventArgs<T>(Item));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Items"></param>
        protected virtual void OnItemsAdded(IEnumerable<T> Items)
        {
            itemsAdded?.Invoke(this, new EventArgs<IEnumerable<object>>(Items.As<IEnumerable>()?.Cast<object>() ?? Enumerable.Empty<object>()));
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
            itemInserted?.Invoke(this, new EventArgs<object>(Item, Index));
            ItemInserted?.Invoke(this, new EventArgs<T>(Item, Index));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        protected virtual void OnItemRemoved(T Item)
        {
            itemRemoved?.Invoke(this, new EventArgs<object>(Item));
            ItemRemoved?.Invoke(this, new EventArgs<T>(Item));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OldItems"></param>
        protected virtual void OnItemsRemoved(IEnumerable<T> OldItems)
        {
            itemsRemoved?.Invoke(this, new EventArgs<IEnumerable<object>>(OldItems.As<IEnumerable>()?.Cast<object>() ?? Enumerable.Empty<object>()));
            ItemsRemoved?.Invoke(this, new EventArgs<IEnumerable<T>>(OldItems));
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnPreviewItemsCleared()
        {
            PreviewItemsCleared?.Invoke(this, new EventArgs());
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

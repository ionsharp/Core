using Imagin.Common.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Imagin.Common.Collections
{
    /// <summary>
    /// Based on List; implements ITrackable.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    public class TrackableList<T> : List<T>, ITrackableCollection<T>
    {
        #region Properties

        #region ITrackableList<T>

        /// <summary>
        /// Occurs when a single item is added.
        /// </summary>
        [field: NonSerializedAttribute()]
        public event EventHandler<EventArgs<T>> ItemAdded;

        /// <summary>
        /// Occurs when any number of items are added.
        /// </summary>
        [field: NonSerializedAttribute()]
        public event EventHandler<EventArgs<IEnumerable<T>>> ItemsAdded;

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        [field: NonSerializedAttribute()]
        public event EventHandler<EventArgs> ItemsChanged;

        /// <summary>
        /// Occurs when the collection is cleared.
        /// </summary>
        [field: NonSerializedAttribute()]
        public event EventHandler<EventArgs> ItemsCleared;

        /// <summary>
        /// Occurs when a single item is inserted.
        /// </summary>
        [field: NonSerializedAttribute()]
        public event EventHandler<EventArgs<T>> ItemInserted;

        /// <summary>
        /// Occurs when a single item is removed.
        /// </summary>
        [field: NonSerializedAttribute()]
        public event EventHandler<EventArgs<T>> ItemRemoved;

        /// <summary>
        /// Occurs when any number of items are removed.
        /// </summary>
        [field: NonSerializedAttribute()]
        public event EventHandler<EventArgs<IEnumerable<T>>> ItemsRemoved;

        bool isEmpty = true;
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

        int total = 0;
        public int Total
        {
            get
            {
                return this.total;
            }
            set
            {
                this.total = value;
                this.OnPropertyChanged("Total");
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Protected

        protected virtual void OnItemAdded(T Item)
        {
            if (this.ItemAdded != null)
                this.ItemAdded(this, new EventArgs<T>(Item));
        }

        protected virtual void OnItemsAdded(IEnumerable<T> Items)
        {
            if (this.ItemsAdded != null)
                this.ItemsAdded(this, new EventArgs<IEnumerable<T>>(Items));
        }

        protected virtual void OnItemsChanged()
        {
            this.Total = this.Count;
            this.IsEmpty = this.Total == 0;

            if (this.ItemsChanged != null)
                this.ItemsChanged(this, new EventArgs());
        }

        protected virtual void OnItemsCleared()
        {
            if (this.ItemsCleared != null)
                this.ItemsCleared(this, new EventArgs());
        }

        protected virtual void OnItemInserted(T Item, int Index)
        {
            if (this.ItemInserted != null)
                this.ItemInserted(this, new EventArgs<T>(Item, Index));
        }

        protected virtual void OnItemRemoved(T Item)
        {
            if (this.ItemRemoved != null)
                this.ItemRemoved(this, new EventArgs<T>(Item));
        }

        protected virtual void OnItemsRemoved(IEnumerable<T> OldItems)
        {
            if (this.ItemsRemoved != null)
                this.ItemsRemoved(this, new EventArgs<IEnumerable<T>>(OldItems));
        }

        #endregion

        #region Public

        public new void Add(T Item)
        {
            base.Add(Item);
            this.OnItemAdded(Item);
            this.OnItemsChanged();
        }

        public void Add(IEnumerable<T> Items)
        {
            foreach (var i in Items)
                base.Add(i);
            this.OnItemsAdded(Items);
            this.OnItemsChanged();
        }

        public new void Clear()
        {
            var Items = this;
            base.Clear();
            this.OnItemsCleared();
            this.OnItemsChanged();
        }

        public new void Insert(int i, T Item)
        {
            base.Insert(i, Item);
            this.OnItemInserted(Item, i);
            this.OnItemsChanged();
        }

        public new void Remove(T Item)
        {
            base.Remove(Item);
            this.OnItemRemoved(Item);
            this.OnItemsChanged();
        }

        public void Remove(IEnumerable<T> Items)
        {
            foreach (var i in Items)
                base.Remove(i);
            this.OnItemsRemoved(Items);
            this.OnItemsChanged();
        }

        public new void RemoveAt(int i)
        {
            var Item = this[i];
            base.RemoveAt(i);
            this.OnItemRemoved(Item);
            this.OnItemsChanged();
        }

        public void RemoveAt(params int[] Indices)
        {
            var Items = new List<T>();
            var j = 0;
            foreach (var i in Indices)
            {
                Items.Add(this[i]);
                base.Remove(Items[j]);
                j++;
            }
            this.OnItemsRemoved(Items);
            this.OnItemsChanged();
        }

        #endregion

        #endregion

        #region TrackableList<T>

        public TrackableList() : base()
        {
        }

        public TrackableList(IEnumerable<T> Items) : base(Items)
        {
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}

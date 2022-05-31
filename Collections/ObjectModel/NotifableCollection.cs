using System;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Imagin.Core.Collections.Generic
{
    [Serializable]
    public class NotifableCollection<T> : ObservableCollection<T>
    {
        public event ChangedEventHandler Changed;

        public NotifableCollection() : base() { }

        void Subscribe(T item)
        {
            Unsubscribe(item);
            if (item is IPropertyChanged i)
                i.PropertyChanged += OnPropertyChanged;
        }

        void Unsubscribe(T item)
        {
            if (item is IPropertyChanged i)
                i.PropertyChanged -= OnPropertyChanged;
        }

        void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) => OnChanged();

        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            foreach (var i in this)
                Subscribe(i);
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Subscribe((T)e.NewItems[0]);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    Unsubscribe((T)e.OldItems[0]);
                    break;
            }
        }

        protected virtual void OnChanged() => Changed?.Invoke(this);

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            OnChanged();
        }
    }
}
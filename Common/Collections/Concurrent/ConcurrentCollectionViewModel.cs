using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Imagin.Common.Collections.Concurrent
{
    /// <summary>
    /// The view model for <see cref="ConcurrentCollectionBase{T}"/>; this is exposed by <see cref="ConcurrentCollectionBase{T}"/> when it is used on the dispatcher thread.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class ConcurrentCollectionViewModel<T> : ObservableCollection<T>, IObserver<NotifyCollectionChangedEventArgs>, IDisposable
    {
        /// <summary>
        /// Token that comes back when subscribing to the <see cref="IObserver{T}"/>.
        /// </summary>
        IDisposable UnsubscribeToken = null;

        /// <summary>
        /// Token for removing the subscription action from the queue
        /// </summary>
        IDisposable SubscriptionActionToken;

        /// <summary>
        /// Processes a NotifyCollectionChangedEventArgs event argument
        /// </summary>
        /// <param name="Command"></param>
        void ProcessCommand(NotifyCollectionChangedEventArgs Command)
        {
            switch (Command.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        var StartIndex = Command.NewStartingIndex;
                        if (StartIndex > -1)
                        {
                            foreach (var i in Command.NewItems)
                            {
                                InsertItem(StartIndex, (T)i);
                                ++StartIndex;
                            }
                        }
                        else
                        {
                            foreach (var i in Command.NewItems)
                            {
                                Add((T)i);
                            }
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    {
                        foreach (T i in Command.OldItems)
                            Remove(i);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    {
                        var StartIndex = Command.OldStartingIndex;
                        foreach (var i in Command.NewItems)
                        {
                            this[StartIndex] = (T)i;
                            ++StartIndex;
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
            }
        }

        /// <summary>
        /// Disposes of this object, and supresses the finalizer
        /// </summary>
        /// <param name="IsDisposing"></param>
        void Dispose(bool IsDisposing)
        {
            if (IsDisposing)
                GC.SuppressFinalize(this);
            if (SubscriptionActionToken != null)
                SubscriptionActionToken.Dispose();
            if (UnsubscribeToken != null)
                UnsubscribeToken.Dispose();
        }

        /// <summary>
        /// Constructor. Queues subscribing to the IObservable<> passed in.
        /// </summary>
        /// <param name="observable"></param>
        /// <remarks>
        /// We create a subscribe action, which has a reference to this object.
        /// If the DispatcherQueueProcessor isn't started (because the Dispatcher hasn't been
        /// created), then the subscriber action will sit in the queue forever, hence will
        /// never be garbage collection, hence this view model will never be garbage
        /// collected. To get around this the dispatcher subscription queue stores a weak
        /// reference. As such the Subscribe Action needs to be referenced in this class
        /// otherwise it will be garbage collected once we leave the scope of this constructor.
        /// the return token holds a reference to the Subscribe Action
        /// </remarks>
        public ConcurrentCollectionViewModel(IObservable<NotifyCollectionChangedEventArgs> Observable)
        {
            SubscriptionActionToken = DispatcherQueueProcessor.Instance.QueueSubscribe(() => UnsubscribeToken = Observable.Subscribe(this));
        }

        /// <summary>
        /// Finalizer, disposes of the object
        /// </summary>
        ~ConcurrentCollectionViewModel()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes of the current object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        ///  IObserver<> implementation
        /// </summary>
        public void OnCompleted()
        {
            //Nothing to do here
        }

        /// <summary>
        ///  IObserver<> implementation
        /// </summary>
        public void OnError(Exception error)
        {
            //Handle it? Log it?
        }

        /// <summary>
        ///  IObserver<> implementation
        /// </summary>
        public void OnNext(NotifyCollectionChangedEventArgs value)
        {
            DispatcherQueueProcessor.Instance.Add(() => ProcessCommand(value));
        }
    }
}

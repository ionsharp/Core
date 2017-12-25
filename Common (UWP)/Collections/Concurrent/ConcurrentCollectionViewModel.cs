using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Imagin.Common.Collections.Concurrent
{
    /// <summary>
    /// The view model for <see cref="ConcurrentCollectionBase{T}"/>; this is exposed by <see cref="ConcurrentCollectionBase{T}"/> when it is used on the dispatcher thread.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConcurrentCollectionViewModel<T> : ConcurrentCollectionViewModelBase<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public override IDispatcherQueueProcessor DispatcherQueueProcessor
        {
            get
            {
                return default(IDispatcherQueueProcessor); //Concurrent.DispatcherQueueProcessor.Instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Observable"></param>
        public ConcurrentCollectionViewModel() : base()
        {
        }
    }
}

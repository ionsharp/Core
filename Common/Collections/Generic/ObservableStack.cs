using System.Collections.ObjectModel;

namespace Imagin.Common.Collections.Generic
{
    /// <summary>
    /// Defines methods relative to stack data strcuture; inherits <see cref="ObservableCollection{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    public class ObservableStack<T> : ObservableCollection<T>, IStackable<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Push(T item)
        {
            Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            return Count > 0 ? this[Count - 1] : default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            if (Count > 0)
            {
                var i = this[Count - 1];
                RemoveAt(Count - 1);
                return i;
            }
            else return default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableStack() : base()
        {
        }
    }
}

using Imagin.Common.Collections.Generic;
using System.Collections.ObjectModel;

namespace Imagin.Common.Collections.ObjectModel
{
    /// <summary>
    /// Based on ObservableCollection, defines methods relative to stack data strcuture.
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
            this.Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            return this.Count > 0 ? this[this.Count - 1] : default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            if (this.Count > 0)
            {
                var i = this[this.Count - 1];
                this.RemoveAt(this.Count - 1);
                return i;
            }
            else return default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableStack()
        {
        }
    }
}

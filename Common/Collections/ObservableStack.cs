using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Imagin.Common.Collections
{
    /// <summary>
    /// Based on ObservableCollection, defines methods relative to stack data strcuture.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    public class ObservableStack<T> : ObservableCollection<T>, IStackable<T>
    {
        #region Methods

        public void Push(T item)
        {
            this.Add(item);
        }

        public T Peek()
        {
            if (this.Count > 0)
            {
                T temp = this[this.Count - 1];
                return temp;
            }
            else
                return default(T);
        }

        public T Pop()
        {
            if (this.Count > 0)
            {
                T temp = this[this.Count - 1];
                this.RemoveAt(this.Count - 1);
                return temp;
            }
            else
                return default(T);
        }

        #endregion

        #region ObservableStack

        public ObservableStack()
        {
        }

        #endregion
    }
}

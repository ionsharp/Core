using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Imagin.Common.Collections
{
    public class Stack<T> : ObservableCollection<T>
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

        public ObservableCollection<T> ToObservableCollection()
        {
            ObservableCollection<T> o = new ObservableCollection<T>();
            foreach (T t in this)
            {
                o.Add(t);
            }
            return o;
        }

        public List<T> ToList()
        {
            return this.ToList<T>();
        }

        #endregion

        #region Stack

        public Stack()
        {
        }

        #endregion
    }
}

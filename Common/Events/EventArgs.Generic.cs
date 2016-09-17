using System;

namespace Imagin.Common.Events
{
    public class EventArgs<T> : EventArgs
    {
        readonly T value;
        public T Value
        {
            get
            {
                return value;
            }
        }

        public EventArgs(T Value)
        {
            this.value = Value;
        }
    }
}

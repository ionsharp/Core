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

        object parameter;
        public object Parameter
        {
            get; set;
        }

        public EventArgs(T Value, object Parameter = null)
        {
            this.value = Value;
            this.parameter = Parameter;
        }
    }
}

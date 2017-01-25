using System;

namespace Imagin.Common.Input
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EventArgs<T> : EventArgs
    {
        readonly T value;
        /// <summary>
        /// 
        /// </summary>
        public T Value
        {
            get
            {
                return value;
            }
        }

        readonly object parameter;
        /// <summary>
        /// 
        /// </summary>
        public object Parameter
        {
            get
            {
                return parameter;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Parameter"></param>
        public EventArgs(T Value, object Parameter = null)
        {
            value = Value;
            parameter = Parameter;
        }
    }
}

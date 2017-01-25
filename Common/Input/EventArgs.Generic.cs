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

        object parameter;
        /// <summary>
        /// 
        /// </summary>
        public object Parameter
        {
            get
            {
                return parameter;
            }
            set
            {
                parameter = value;
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

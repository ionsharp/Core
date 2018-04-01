using System;

namespace Imagin.Common.Input
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EventArgs<T> : EventArgs
    {
        readonly T _value;
        /// <summary>
        /// 
        /// </summary>
        public T Value => _value;

        readonly object _parameter;
        /// <summary>
        /// 
        /// </summary>
        public object Parameter => _parameter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameter"></param>
        public EventArgs(T value, object parameter = null)
        {
            _value = value;
            _parameter = parameter;
        }
    }
}

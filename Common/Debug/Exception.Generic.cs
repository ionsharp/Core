using System;

namespace Imagin.Common.Debug
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Exception<T> : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public T Value
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Exception() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Message"></param>
        /// <param name="InnerException"></param>
        public Exception(T Value, string Message = "", Exception InnerException = null) : base(Message, InnerException)
        {
            this.Value = Value;
        }
    }
}

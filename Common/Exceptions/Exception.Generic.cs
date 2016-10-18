using System;

namespace Imagin.Common.Exceptions
{
    public class Exception<T> : Exception
    {
        public T Value
        {
            get; set;
        }

        public Exception() : base()
        {
        }

        public Exception(T Value, string Message = "", Exception InnerException = null) : base(Message, InnerException)
        {
            this.Value = Value;
        }
    }
}

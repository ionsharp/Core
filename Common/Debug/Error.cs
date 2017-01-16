using System;

namespace Imagin.Common.Debug
{
    public class Error<T> : Error
    {
        public new T Data
        {
            get; set;
        }

        public Error() : base()
        {
        }

        public Error(Exception Exception) : base(Exception)
        {
        }

        public Error(Exception Exception, T Data) : base(Exception, Data)
        {
        }

        public Error(string Message) : base(Message)
        {
        }

        public Error(string Message, T Data) : base(Message, Data)
        {
        }
    }

    /// <summary>
    /// Represents a failed result; optionally, encapsulates an exception.
    /// </summary>
    public class Error : Result
    {
        public Exception Exception
        {
            get; set;
        }

        public Error() : this(string.Empty)
        {
        }

        public Error(Exception Exception) : this(Exception, null)
        {
        }

        public Error(Exception exception, object data) : base(data)
        {
            Exception = exception;
        }

        public Error(string Message) : this(Message, null)
        {
        }

        public Error(string Message, object data) : this(new Exception<string>(Message), null)
        {
        }
    }
}

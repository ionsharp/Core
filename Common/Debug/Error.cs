using System;

namespace Imagin.Common.Debug
{
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

        public Error(Exception exception, object data = null) : base(data)
        {
            Exception = exception;
        }

        public Error(string Message, object data = null) : this(new Exception<string>(Message), data)
        {
        }
    }
}

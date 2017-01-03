using System;
using System.Windows.Input;
using Imagin.Common.Primitives;

namespace Imagin.Common.Debug
{
    /// <summary>
    /// Represents a failed result; optionally, encapsulates an exception.
    /// </summary>
    public class Error : Result
    {
        Exception exception = default(Exception);
        public Exception Exception
        {
            get
            {
                return exception;
            }
            set
            {
                exception = value;
            }
        }

        public Error() : base()
        {
        }

        public Error(object Data) : base(Data)
        {
        }

        public Error(Exception exception) : base()
        {
            Exception = exception;
        }

        public Error(Exception exception, object Data) : base(Data)
        {
            Exception = exception;
        }

        public Error(string Message) : base()
        {
            Exception = new Exception<string>(Message);
        }

        public Error(string Message, object Data) : base(Data)
        {
            Exception = new Exception<string>(Message);
        }
    }
}

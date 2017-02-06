using System;

namespace Imagin.Common.Debug
{
    /// <summary>
    /// Represents a failed result; optionally, encapsulates an exception.
    /// </summary>
    public class Error : Result
    {
        /// <summary>
        /// 
        /// </summary>
        public Exception Exception
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Exception?.Message ?? Data.ToString() ?? string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        public Error() : this(string.Empty)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="data"></param>
        public Error(Exception exception, object data = null) : base(data)
        {
            Exception = exception;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="data"></param>
        public Error(string Message, object data = null) : this(new Exception<string>(Message), data)
        {
        }
    }
}

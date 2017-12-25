using System;

namespace Imagin.Common.Debug
{
    /// <summary>
    /// Represents a failed result; optionally, encapsulates an exception.
    /// </summary>
    public class Error<TData> : Result<TData>
    {
        readonly Exception exception;
        /// <summary>
        /// 
        /// </summary>
        public Exception Exception
        {
            get
            {
                return exception;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public Exception InnerException
        {
            get
            {
                return exception?.InnerException;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Message
        {
            get
            {
                return exception?.Message;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Message ?? Data.ToString() ?? string.Empty;
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
        /// <param name="Exception"></param>
        public Error(Exception Exception) : this(Exception, default(TData))
        {
            exception = Exception;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Exception"></param>
        /// <param name="Data"></param>
        public Error(Exception Exception, TData Data) : base(Data)
        {
            exception = Exception;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        public Error(string Message) : this(Message, default(TData))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Data"></param>
        public Error(string Message, TData Data) : this(new Exception(Message), Data)
        {
        }
    }
}

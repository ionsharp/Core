using System;

namespace Imagin.Common.Debug
{
    /// <summary>
    /// Represents a failed result; optionally, encapsulates an exception.
    /// </summary>
    public class Error : Error<object>
    {
        /// <summary>
        /// 
        /// </summary>
        public Error() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Exception"></param>
        public Error(Exception Exception) : base(Exception)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Exception"></param>
        /// <param name="Data"></param>
        public Error(Exception Exception, object Data) : base(Exception, Data)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        public Error(string Message) : base(Message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Data"></param>
        public Error(string Message, object Data) : base(Message, Data)
        {
        }
    }
}

using System;

namespace Imagin.Common.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class InvalidTokenException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public InvalidTokenException() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        public InvalidTokenException(string Message) : base(Message)
        {
        }
    }
}

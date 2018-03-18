using System;

namespace Imagin.Common.Debug
{
    /// <summary>
    /// 
    /// </summary>
    public class InvalidResultException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public InvalidResultException() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        public InvalidResultException(string Message) : base(Message)
        {
        }
    }
}

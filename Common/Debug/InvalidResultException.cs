using System;

namespace Imagin.Common.Debug
{
    public class InvalidResultException : Exception
    {
        public InvalidResultException() : base()
        {
        }

        public InvalidResultException(string Message) : base(Message)
        {
        }
    }
}

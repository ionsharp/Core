using System;

namespace Imagin.Common.Exceptions
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

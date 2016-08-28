using System;

namespace Imagin.Common.Exceptions
{
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException() : base()
        {
        }
        public InvalidTokenException(string Message) : base(Message)
        {
        }
    }
}

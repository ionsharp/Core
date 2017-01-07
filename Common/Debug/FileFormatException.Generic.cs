using Imagin.Common.Debug;
using System;

namespace Imagin.Common.Exceptions
{
    public class FileFormatException<T> : Exception<T>
    {
        public FileFormatException(T Value, string Message = "", Exception InnerException = null) : base(Value, Message, InnerException)
        {
        }
    }
}

using Imagin.Common.Debug;
using System;

namespace Imagin.Common.Exceptions
{
    public class FileNotFoundException<T> : Exception<T>
    {
        public FileNotFoundException(T Value, string Message = "", Exception InnerException = null) : base(Value, Message, InnerException)
        {
        }
    }
}

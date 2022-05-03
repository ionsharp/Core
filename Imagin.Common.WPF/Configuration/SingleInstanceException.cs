using System;

namespace Imagin.Common.Configuration
{
    /// <summary>
    /// The exception that is thrown when more than one instance of an object is created.
    /// </summary>
    public class SingleInstanceException : Exception
    {
        public SingleInstanceException() : base("Only one instance can exist.") { }
    }
}
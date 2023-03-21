using System;

namespace Imagin.Core.Analytics;

public class Exception<T> : Exception
{
    public readonly T Value;

    public Exception() : base() { }

    public Exception(T value, string message = "", Exception innerException = null) : base(message, innerException)
    {
        Value = value;
    }
}
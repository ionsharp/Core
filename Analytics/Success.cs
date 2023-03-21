using System;

namespace Imagin.Core.Analytics;

[Serializable]
public class Success : Success<object>
{
    public Success() : base() { }

    public Success(object data, object text = null) : base(data, text) { }

    public Success(string text) : this(null, text) { }
}
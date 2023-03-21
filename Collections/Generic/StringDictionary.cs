using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Imagin.Core.Collections.Generic;

[Serializable]
public class StringDictionary : Dictionary<string, string> 
{
    protected StringDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public StringDictionary() : base() { }
}
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Imagin.Core.Collections.Generic;

[Serializable]
public class ObjectDictionary : Dictionary<string, object>
{
    protected ObjectDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public ObjectDictionary() : base() { }
}
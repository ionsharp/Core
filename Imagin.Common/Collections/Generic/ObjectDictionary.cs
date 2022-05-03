using System;
using System.Collections.Generic;

namespace Imagin.Common.Collections.Generic
{
    [Serializable]
    public class ObjectDictionary : Dictionary<string, object>
    {
        public ObjectDictionary() : base() { }
    }
}
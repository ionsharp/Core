using System;

namespace Imagin.Common.Analytics
{
    public class MemberMissingAttributeWarning<T> : Warning where T : Attribute
    {
        public MemberMissingAttributeWarning(string name, Type type) : base($"Member '{name}' of type '{type.FullName}' is missing '{typeof(T).FullName}' attribute.") { }
    }
}
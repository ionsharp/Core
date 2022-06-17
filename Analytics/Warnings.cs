using System;

namespace Imagin.Core.Analytics;

public class MemberMissingAttributeWarning<T> : Warning where T : Attribute
{
    public MemberMissingAttributeWarning(string name, Type type) : base($"Member '{name}' of type '{type.FullName}' is missing '{typeof(T).FullName}' attribute.") { }
}

public class NotSerializableWarning : Warning
{
    public NotSerializableWarning(object input) : base($"'{input.GetType().FullName}' is not marked as serializable.") { }
}
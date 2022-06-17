using System;

namespace Imagin.Core;

public class ChildNotFoundException<Child, Parent> : Exception
{
    public ChildNotFoundException() : base($"'{typeof(Parent).FullName}' must have logical or visual child of type '{typeof(Child).FullName}'.") { }
}

public class ExternalChangeException<Object> : Exception
{
    public ExternalChangeException(string propertyName) : base($"External changes to '{typeof(Object).FullName}.{propertyName}' are not allowed.") { }
}

public class InvalidAncestor<Target> : Exception
{
    public InvalidAncestor() : base($"'{typeof(Target).FullName}' must be an ancestor.") { }
}

public class ParentNotFoundException<Child, Parent> : Exception
{
    public ParentNotFoundException() : base($"'{typeof(Child).FullName}' must have logical or visual parent of type '{typeof(Parent).FullName}'.") { }
}
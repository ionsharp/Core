using System;

namespace Imagin.Core;

public class ChildNotFoundException<Child, Parent> : Exception
{
    public ChildNotFoundException() : base($"'{typeof(Parent).FullName}' must have logical or visual child of type '{typeof(Child).FullName}'.") { }
}

public class DeserializationFailedException : Exception
{
    public DeserializationFailedException(Type type, Exception inner = null) : base($"'{type.FullName}' failed to deserialize.", inner) { }
}

public class ExternalChangeException<Object> : Exception
{
    public ExternalChangeException(string propertyName) : base($"External changes to '{typeof(Object).FullName}.{propertyName}' are not allowed.") { }
}

public class FileNotSupported : Exception
{
    public FileNotSupported(string filePath) : base($"The file '{filePath}' is not supported.") { }
}

public class InvalidAncestor<Target> : Exception
{
    public InvalidAncestor() : base($"'{typeof(Target).FullName}' must be an ancestor.") { }
}

public class InvalidFileException : Exception
{
    public InvalidFileException(string filePath) : base($"The file '{filePath}' is invalid or corrupt.") { }
}

public class ParentNotFoundException<Child, Parent> : Exception
{
    public ParentNotFoundException() : base($"'{typeof(Child).FullName}' must have logical or visual parent of type '{typeof(Parent).FullName}'.") { }
}

public class NotCloneableException<T> : Exception
{
    public NotCloneableException() : base($"'{typeof(T).FullName}' is not cloneable.") { }
}

public class SerializationFailedException : Exception
{
    public SerializationFailedException(Type type, Exception inner = null) : base($"'{type.FullName}' failed to serialize.", inner) { }
}

[Name("Wrong password")]
public class WrongPasswordException : Exception
{
    public WrongPasswordException() : base("The password entered is incorrect.") { }
}
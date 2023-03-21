using System;

namespace Imagin.Core.Models;

/// <inheritdoc/>
[Serializable]
public class ViewModel : Base
{
    public ViewModel() : base() { }
}

/// <inheritdoc/>
[Serializable]
public class NamableViewModel : Namable
{
    public NamableViewModel() : base() { }
}

/// <inheritdoc/>
[Serializable]
public class LockableViewModel : Lockable
{
    public LockableViewModel() : base() { }
}
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
public class LockableViewModel : BaseLockable
{
    public LockableViewModel() : base() { }
}
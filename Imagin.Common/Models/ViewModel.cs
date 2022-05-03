using System;

namespace Imagin.Common.Models
{
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
}
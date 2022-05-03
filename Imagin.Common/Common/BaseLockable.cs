using Imagin.Common.Input;
using System;

namespace Imagin.Common
{
    /// <summary>
    /// Specifies an <see cref="object"/> that locks and unlocks.
    /// </summary>
    [Serializable]
    public class BaseLockable : Base, ILock
    {
        [field: NonSerialized]
        public event LockedEventHandler Locked;

        bool isLocked = false;
        [Hidden]
        [Serialize(false)]
        public virtual bool IsLocked
        {
            get => isLocked;
            set
            {
                this.Change(ref isLocked, value);
                OnLocked(value);
            }
        }

        public BaseLockable() : base() { }

        public virtual void OnLocked(bool isLocked) => Locked?.Invoke(this, new LockedEventArgs(isLocked));
    }
}

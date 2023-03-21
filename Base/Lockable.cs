using Imagin.Core.Input;
using System;

namespace Imagin.Core;

/// <summary>Specifies an <see cref="object"/> that locks and unlocks.</summary>
[Serializable]
public class Lockable : Base, ILock
{
    [field: NonSerialized]
    public event LockedEventHandler Locked;

    [Hide, NonSerializable]
    public virtual bool IsLocked { get => Get(false); set => Set(value); }

    public Lockable() : base() { }

    public override void OnPropertyChanged(PropertyEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.PropertyName == nameof(IsLocked))
            OnLocked(IsLocked);
    }

    [Hide]
    public virtual void OnLocked(bool isLocked) => Locked?.Invoke(this, new LockedEventArgs(isLocked));
}
using Imagin.Core.Input;

namespace Imagin.Core;

/// <summary>
/// Specifies an <see cref="object"/> that can be locked and unlocked.
/// </summary>
public interface ILock
{
    event LockedEventHandler Locked;

    bool IsLocked
    {
        get; set;
    }
}

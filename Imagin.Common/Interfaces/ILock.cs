using Imagin.Common.Input;

namespace Imagin.Common
{
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
}

using System;

namespace Imagin.Common
{
    /// <summary>
    /// Specifies an <see cref="object"/> that can be locked and unlocked.
    /// </summary>
    public interface ILockable
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EventArgs> Locked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EventArgs> Unlocked;

        /// <summary>
        /// 
        /// </summary>
        bool IsLocked
        {
            get; set;
        }
    }
}

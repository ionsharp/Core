using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
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

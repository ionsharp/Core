using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDeletable
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EventArgs> Deleted;

        /// <summary>
        /// 
        /// </summary>
        void OnDeleted();
    }
}

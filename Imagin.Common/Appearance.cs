using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum Appearance
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        Hidden = 1,
        /// <summary>
        /// 
        /// </summary>
        Visible = 2,
        /// <summary>
        /// 
        /// </summary>
        All = Hidden | Visible
    }
}
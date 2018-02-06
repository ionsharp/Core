using System;

namespace Imagin.Common.Input
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum MouseEvent
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        Default = 1,
        /// <summary>
        /// 
        /// </summary>
        DelayedMouseDown = 2,
        /// <summary>
        /// 
        /// </summary>
        MouseDown = 4,
        /// <summary>
        /// 
        /// </summary>
        MouseUp = 8,
        /// <summary>
        /// 
        /// </summary>
        MouseDoubleClick = 16
    }
}

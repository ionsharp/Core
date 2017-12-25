using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum Side
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        Top = 1,
        /// <summary>
        /// 
        /// </summary>
        Bottom = 2,
        /// <summary>
        /// 
        /// </summary>
        Left = 4,
        /// <summary>
        /// 
        /// </summary>
        Right = 8,
        /// <summary>
        /// 
        /// </summary>
        All = Top | Bottom | Left | Right
    }
}

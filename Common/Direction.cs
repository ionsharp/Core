using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum Direction
    {
        /// <summary>
        /// 
        /// </summary>
        None = 1,
        /// <summary>
        /// 
        /// </summary>
        Up = 2,
        /// <summary>
        /// 
        /// </summary>
        Down = 4,
        /// <summary>
        /// 
        /// </summary>
        Left = 8,
        /// <summary>
        /// 
        /// </summary>
        Right = 16,
        /// <summary>
        /// 
        /// </summary>
        All = Up | Down | Left | Right
    }
}

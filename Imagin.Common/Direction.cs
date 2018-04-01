using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum RelativeDirection
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        Up = 1,
        /// <summary>
        /// 
        /// </summary>
        Down = 2,
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
        All = Up | Down | Left | Right
    }
}

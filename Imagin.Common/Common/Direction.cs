using System;

namespace Imagin.Common
{
    [Flags]
    [Serializable]
    public enum RelativeDirection
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8,
        All = Up | Down | Left | Right
    }
}
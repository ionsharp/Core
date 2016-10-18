using System;

namespace Imagin.Common
{
    [Flags]
    [Serializable]
    public enum Direction
    {
        None = 1,
        Up = 2,
        Down = 4,
        Left = 8,
        Right = 16,
        All = Up | Down | Left | Right
    }
}

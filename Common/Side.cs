using System;

namespace Imagin.Common
{
    [Flags]
    [Serializable]
    public enum Side
    {
        None = 1,
        Top = 2,
        Bottom = 4,
        Left = 8,
        Right = 16,
        All = Top | Bottom | Left | Right
    }
}

using Imagin.Common;
using System;

namespace Imagin.Apps.Paint
{
    [Flags]
    [Serializable]
    public enum LayerType
    {
        [Hidden]
        None = 0,
        Effect = 1,
        Group = 2,
        Pixel = 4,
        Shape = 8,
        Static = 16,
        Text = 32,
        [Hidden]
        All = Effect | Group | Pixel | Shape | Static | Text
    }
}
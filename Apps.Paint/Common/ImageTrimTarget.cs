using Imagin.Common;
using System;

namespace Imagin.Apps.Paint
{
    [Flags]
    public enum ImageTrimTarget
    {
        [Hidden]
        None = 0,
        Top = 1,
        Left = 2,
        Right = 4,
        Bottom = 8,
        [Hidden]
        All = Top | Left | Right | Bottom
    }
}
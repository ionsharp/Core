using System;

namespace Imagin.Common.Input
{
    [Flags]
    public enum MouseEvent
    {
        None = 0,
        DelayedMouseDown = 1,
        MouseDown = 2,
        MouseUp = 4,
        MouseDoubleClick = 8
    }
}
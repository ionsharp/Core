using System;

namespace Imagin.Common.Input
{
    [Flags]
    public enum MouseEvent
    {
        None = 0,
        Default = 1,
        DelayedMouseDown = 2,
        MouseDown = 4,
        MouseUp = 8,
        MouseDoubleClick = 16
    }
}

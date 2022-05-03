using System;

namespace Imagin.Common.Controls
{
    [Flags]
    [Serializable]
    public enum PopupTriggers
    {
        [Hidden]
        None = 0,
        GotFocus = 1,
        GotKeyboardFocus = 2,
        TextChanged = 4,
        [Hidden]
        All = GotFocus | GotKeyboardFocus | TextChanged
    }
}
using Imagin.Common;
using System;

namespace Imagin.Apps.Paint
{
    [Flags]
    [Serializable]
    public enum BlendDirections : int
    {
        [Hidden]
        None = 0,
        Horizontal = 1,
        Vertical = 2,
        [Hidden]
        Both = Horizontal | Vertical
    }
}
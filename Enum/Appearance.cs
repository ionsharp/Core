using System;

namespace Imagin.Core;

[Flags, Serializable]
public enum Appearance
{
    None = 0, Hidden = 1, Visible = 2,
    All = Hidden | Visible
}
using System;

namespace Imagin.Common.Web
{
    [Flags]
    public enum ServerObjectProperty
    {
        None = 1,
        Name = 2,
        Path = 4,
        Contents = 8,
        Extension = 16,
        Type = 32,
        Size = 64,
        Accessed = 128,
        Created = 256,
        Modified = 512,
        Permissions = 1024,
        All = Name | Path | Contents | Extension | Type | Size | Accessed | Created | Modified | Permissions
    }
}

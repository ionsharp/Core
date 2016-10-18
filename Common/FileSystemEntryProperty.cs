using System;

namespace Imagin.Common
{
    [Flags]
    public enum FileSystemEntryProperty
    {
        None = 1,
        Name = 2,
        Path = 4,
        Extension = 8,
        Type = 16,
        Size = 32,
        Accessed = 64,
        Created = 128,
        Modified = 256,
        Permissions = 512,
        All = Name | Path | Extension | Type | Size | Accessed | Created | Modified | Permissions
    }
}

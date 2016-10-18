using System;

namespace Imagin.Common
{
    [Flags]
    [Serializable]
    public enum FileSystemEntry
    {
        File = 1,
        Folder = 2,
        Link = 4,
        Drive = 8,
        Null = 16,
        All = File | Folder | Link | Drive
    }
}

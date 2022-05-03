using System;

namespace Imagin.Common.Storage
{
    [Flags]
    [Serializable]
    public enum ItemType
    {
        [Hidden]
        Nothing = 0,
        File = 1,
        Folder = 2,
        Shortcut = 4,
        Drive = 8,
        Root = 16,
        [Hidden]
        All = File | Folder | Shortcut | Drive | Root
    }
}

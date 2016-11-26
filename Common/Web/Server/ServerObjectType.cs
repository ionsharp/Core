using System;

namespace Imagin.Common.Web
{
    [Flags]
    [Serializable]
    public enum ServerObjectType
    {
        File = 1,
        Folder = 2,
        Link = 4,
        Drive = 8,
        Null = 16,
        All = File | Folder | Link | Drive
    }
}

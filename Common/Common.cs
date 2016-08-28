using System;

namespace Imagin.Common
{
    public enum CompassDirection
    {
        N,
        S,
        W,
        E,
        Origin,
        NW,
        NE,
        SW,
        SE
    }

    [Flags]
    public enum Direction
    {
        None = 1,
        Up = 2,
        Down = 4,
        Left = 8,
        Right = 16,
        All = Up | Down | Left | Right
    }

    [Flags]
    public enum FileSystemEntry
    {
        File = 1,
        Folder = 2,
        Link = 4,
        Drive = 8,
        Null = 16,
        All = File | Folder | Link | Drive
    }

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

    [Flags]
    public enum Side
    {
        None = 1,
        Top = 2,
        Bottom = 4,
        Left = 8,
        Right = 16,
        All = Top | Bottom | Left | Right
    }
}
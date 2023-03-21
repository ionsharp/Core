using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class FileAttribute : Attribute
{
    public string Extension { get; set; }

    public string Name { get; set; }

    public FileAttribute(string name = "", string extension = "")
    {
        Name = name; Extension = extension;
    }
}
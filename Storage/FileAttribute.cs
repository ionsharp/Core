using System;

namespace Imagin.Core
{
    public class FileAttribute : Attribute
    {
        public string Extension { get; set; }

        public readonly string Name;

        public FileAttribute(string name = "", string extension = "")
        {
            Name = name;
            Extension = extension;
        }
    }
}
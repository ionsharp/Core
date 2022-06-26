using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class FilePathAttribute : Attribute
{
    public FilePathAttribute() : base() { }
}
using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class FolderPathAttribute : Attribute
{
    public FolderPathAttribute() : base() { }
}
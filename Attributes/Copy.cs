using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class CopyAttribute : Attribute
{
    public CopyAttribute() : base() { }
}
using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class UpDownAttribute : Attribute
{
    public UpDownAttribute() : base() { }
}
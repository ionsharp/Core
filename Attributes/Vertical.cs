using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class VerticalAttribute : Attribute
{
    public VerticalAttribute() : base() { }
}
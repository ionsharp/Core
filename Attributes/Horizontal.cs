using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class HorizontalAttribute : Attribute
{
    public HorizontalAttribute() : base() { }
}
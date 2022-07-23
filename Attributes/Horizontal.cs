using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Struct)]
public class HorizontalAttribute : Attribute
{
    public HorizontalAttribute() : base() { }
}
using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class UnitAttribute : Attribute
{
    public UnitAttribute() : base() { }
}
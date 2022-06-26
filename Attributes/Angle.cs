using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class AngleAttribute : Attribute
{
    public AngleAttribute() : base() { }
}
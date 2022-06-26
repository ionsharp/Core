using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ToggleButtonAttribute : Attribute
{
    public ToggleButtonAttribute() : base() { }
}
using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class SelectedIndexAttribute : Attribute
{
    public SelectedIndexAttribute() : base() { }
}
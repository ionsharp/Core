using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class BelowAttribute : Attribute
{
    public BelowAttribute() : base() { }
}
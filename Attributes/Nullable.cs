using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class NullableAttribute : Attribute
{
    public NullableAttribute() { }
}
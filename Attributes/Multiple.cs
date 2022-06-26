using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class MultipleAttribute : Attribute
{
    public MultipleAttribute() : base() { }
}
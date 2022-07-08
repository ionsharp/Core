using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ObjectAttribute : Attribute
{
    public ObjectAttribute() : base() { }
}
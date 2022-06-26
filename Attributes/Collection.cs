using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class CollectionAttribute : Attribute
{
    public CollectionAttribute() : base() { }
}
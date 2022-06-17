using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public abstract class Attribute<T> : Attribute
{
    public readonly T Value;

    public Attribute(T input) : base() => Value = input;
}
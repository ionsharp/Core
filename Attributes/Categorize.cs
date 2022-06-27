using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class CategorizeAttribute : Attribute
{
    public readonly bool Categorize;

    public CategorizeAttribute(bool Categorize = true) : base() => this.Categorize = Categorize;
}
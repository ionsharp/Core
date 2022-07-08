using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class AboveAttribute : Attribute
{
    public AboveAttribute() : base() { }
}
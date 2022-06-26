using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class CommasAttribute : Attribute
{
    public CommasAttribute() : base() { }
}
using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ClearAttribute : Attribute<bool>
{
    public ClearAttribute(bool input) : base(input) { }
}
using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class BulletsAttribute : Attribute
{
    public BulletsAttribute() : base() { }
}
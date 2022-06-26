using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ButtonAttribute : Attribute
{
    public ButtonAttribute() : base() { }
}
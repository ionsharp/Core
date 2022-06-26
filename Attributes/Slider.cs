using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class SliderAttribute : Attribute
{
    public SliderAttribute() : base() { }
}
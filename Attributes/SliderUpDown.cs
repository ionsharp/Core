using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class SliderUpDownAttribute : Attribute
{
    public SliderUpDownAttribute() : base() { }
}
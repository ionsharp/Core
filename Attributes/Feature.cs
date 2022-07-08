using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class FeatureAttribute : Attribute
{
    public readonly bool Featured;

    public readonly AboveBelow Where;

    public FeatureAttribute() : this(true) { }

    public FeatureAttribute(AboveBelow where) : this(true, where) { }

    public FeatureAttribute(bool featured, AboveBelow where = AboveBelow.Above) : base()
    {
        Featured = featured;
        Where = where;
    }
}
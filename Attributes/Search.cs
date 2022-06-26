using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class SearchAttribute : Attribute
{
    public SearchAttribute() : base() { }
}
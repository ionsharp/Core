using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ThumbnailAttribute : Attribute
{
    public ThumbnailAttribute() : base() { }
}
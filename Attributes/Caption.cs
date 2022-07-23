using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class CaptionAttribute : Attribute
{
    public readonly string Caption;

    public CaptionAttribute(string Caption) : base() => this.Caption = Caption;
}
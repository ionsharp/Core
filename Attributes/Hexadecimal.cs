using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class HexadecimalAttribute : Attribute
{
    public HexadecimalAttribute() : base() { }
}
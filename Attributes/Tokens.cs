using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class TokensAttribute : Attribute
{
    public TokensAttribute() : base() { }
}
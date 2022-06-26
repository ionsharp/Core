using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class PasswordAttribute : Attribute
{
    public PasswordAttribute() : base() { }
}
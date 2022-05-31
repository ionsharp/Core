using System;

namespace Imagin.Core.Local;

[AttributeUsage(AttributeTargets.Field)]
public class CultureAttribute : Attribute
{
    public readonly string Code;

    public CultureAttribute(string code) : base() => Code = code;
}
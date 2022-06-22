using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Class)]
public class IgnoreAttribute : Attribute
{
    public readonly string[] Values;

    public IgnoreAttribute(params string[] values) : base() => Values = values;
}
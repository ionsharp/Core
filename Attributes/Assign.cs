using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class AssignAttribute : Attribute
{
    public readonly string Values = null;

    public readonly Type[] Types = null;

    public AssignAttribute(params Type[] types) : base() => Types = types;

    public AssignAttribute(string values) : base() => Values = values;
}
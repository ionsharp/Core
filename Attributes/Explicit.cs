using System;

namespace Imagin.Core;

/// <summary>The member is hidden unless <see cref="HiddenAttribute"/> or <see cref="VisibleAttribute"/> is otherwise specified.</summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class ExplicitAttribute : Attribute
{
    public ExplicitAttribute() : base() { }
}
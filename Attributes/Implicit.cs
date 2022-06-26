using System;

namespace Imagin.Core;

/// <summary>The member is visible unless <see cref="HiddenAttribute"/> or <see cref="VisibleAttribute"/> is otherwise specified.</summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class ImplicitAttribute : Attribute
{
    public ImplicitAttribute() : base() { }
}
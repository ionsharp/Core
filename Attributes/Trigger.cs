using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class TriggerAttribute : Attribute
{
    /// <summary>The property to get a value from on the defining object (this can be anything).</summary>
    public readonly string SourceName;

    /// <summary>The property to set a value to (any property defined by <see cref="Controls.MemberModel"/>).</summary>
    public readonly string TargetName;

    public TriggerAttribute(string targetName, string sourceName) : base()
    {
        TargetName = targetName; SourceName = sourceName;
    }
}
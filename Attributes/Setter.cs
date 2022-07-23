using System;

namespace Imagin.Core;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class SetterAttribute : Attribute
{
    public readonly string PropertyName;

    public readonly object Value;

    public SetterAttribute(string propertyName, object value) : base()
    {
        PropertyName = propertyName; Value = value;
    }
}
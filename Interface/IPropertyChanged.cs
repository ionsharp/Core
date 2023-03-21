using Imagin.Core.Collections.Generic;
using System;
using System.ComponentModel;

namespace Imagin.Core;

/// <summary>Notifies clients that a property value has changed.</summary>
/// <remarks>Implements <see cref="INotifyPropertyChanged"/>.</remarks>
public interface IPropertyChanged : INotifyPropertyChanged
{
    ObjectDictionary SerializedProperties { get; }

    ObjectDictionary NonSerializedProperties { get; }

    /// <summary>Occurs when a property value changes.</summary>
    void OnPropertyChanged(PropertyEventArgs e);

    /// <summary>Occurs before a property changes.</summary>
    void OnPropertyChanging(PropertyChangingEventArgs e);

    /// <summary>Occurs when a property value is requested.</summary>
    void OnPropertyGet(GetPropertyEventArgs e);
}

public class PropertyEventArgs : EventArgs
{
    public readonly string PropertyName;

    public readonly object OldValue;

    public object NewValue { get; set; }

    public PropertyEventArgs(string propertyName, object oldValue, object newValue) : base()
    {
        PropertyName = propertyName;
        OldValue = oldValue; NewValue = newValue;
    }
}

public class PropertyChangingEventArgs : PropertyEventArgs
{
    public bool Cancel { get; set; } = false;

    public PropertyChangingEventArgs(string propertyName, object oldValue, object newValue) : base(propertyName, oldValue, newValue) { }
}

public class GetPropertyEventArgs : EventArgs
{
    public readonly string PropertyName;

    public object Value { get; set; }

    public GetPropertyEventArgs(string propertyName, object value) : base() { PropertyName = propertyName; Value = value; }
}


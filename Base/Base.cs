using Imagin.Core.Collections.Generic;
using Imagin.Core.Conversion;
using Imagin.Core.Input;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Imagin.Core;

/// <summary>Specifies an <see cref="object"/> that implements <see cref="IPropertyChanged"/>.</summary>
[Serializable]
public abstract class Base : object, IModify, IPropertyChanged
{
    [NonSerialized]
    ObjectDictionary NonSerializedProperties = new();
    ObjectDictionary IPropertyChanged.NonSerializedProperties => NonSerializedProperties;

    readonly ObjectDictionary SerializedProperties = new();
    ObjectDictionary IPropertyChanged.SerializedProperties => SerializedProperties;

    ///

    [Hide]
    [field: NonSerialized]
    public event ModifiedEventHandler Modified;
    event ModifiedEventHandler IModify.Modified { add => Modified += value; remove => Modified -= value; }

    [Hide]
    [field: NonSerialized]
    public event PropertyChangedEventHandler PropertyChanged;

    ///

    [Hide, XmlIgnore]
    public virtual bool IsModified { get => Get(false, false); set => Set(value, false); }

    ///

    [OnDeserialized]
    void OnDeserialized(StreamingContext input) => NonSerializedProperties ??= new();

    ///

    [Hide]
    public virtual void OnModified(ModifiedEventArgs e)
    {
        IsModified = true;
        Modified?.Invoke(this, e);
    }

    [Hide]
    public virtual void OnPropertyChanged(PropertyEventArgs e) => PropertyChanged?.Invoke(this, new(e.PropertyName));
    
    [Hide]
    public virtual void OnPropertyChanging(PropertyChangingEventArgs e) { }

    [Hide]
    public virtual void OnPropertyGet(GetPropertyEventArgs e) { }

    ///

    public T Get<T>(T defaultValue = default, bool serialize = true, [CallerMemberName] string propertyName = "")
        => XPropertyChanged.Get(this, defaultValue, serialize, propertyName);

    ///

    public A GetFrom<A, B>(A defaultValue, IConvert<A, B> convert, bool serialize = true, [CallerMemberName] string propertyName = "")
        => XPropertyChanged.GetFrom(this, defaultValue, convert, serialize, propertyName);

    public T GetFromString<T>(T defaultValue, bool serializable = true, [CallerMemberName] string propertyName = "") where T : Enum
        => XPropertyChanged.GetFromString(this, defaultValue, serializable, propertyName);

    ///

    public bool Set<T>(T newValue, bool serialize = true, bool handle = false, [CallerMemberName] string propertyName = "")
        => XPropertyChanged.Set(this, newValue, serialize, handle, propertyName);

    public bool Set<T>(Expression<Func<T>> propertyName, T value, bool serialize = true, bool handle = false)
        => XPropertyChanged.Set(this, propertyName, value, serialize, handle);

    ///

    public bool SetFrom<A, B>(A newValue, IConvert<A, B> convert, bool serialize = true, bool handle = false, [CallerMemberName] string propertyName = "")
        => XPropertyChanged.SetFrom(this, newValue, convert, serialize, handle, propertyName);

    public bool SetFromString<T>(T newValue, bool serialize = true, bool handle = false, [CallerMemberName] string propertyName = "") where T : Enum
        => XPropertyChanged.SetFromString(this, newValue, serialize, handle, propertyName);

    [Hide]
    public void Update<T>(Expression<Func<T>> propertyName) 
        => XPropertyChanged.Update(this, propertyName);
}

/// <inheritdoc/>
[Serializable]
public abstract class Base<T> : Base
{
    public virtual T Value { get => Get<T>(default); set => Set(value); }

    public Base() : base() { }

    public Base(T value) : this() => Value = value;
}
using Imagin.Core.Collections.ObjectModel;
using Imagin.Core.Input;
using System;

namespace Imagin.Core;

#region Namable

/// <summary>Specifies an <see cref="object"/> that implements <see cref="IName"/>.</summary>
[Serializable]
public class Namable : Base, IName
{
    public const string DefaultName = "Untitled";

    [field: NonSerialized]
    public event EventHandler<EventArgs<string>> NameChanged;

    [Assign(nameof(DefaultNames)), Pin(Pin.AboveOrLeft), Horizontal, Index(0), Modify]
    public virtual string Name { get => Get(""); set => Set(value); }

    [Hide]
    public object DefaultNames => new ObservableCollection<string>() { DefaultName };

    public Namable() : base() { }

    public Namable(string name) : base() => Name = name;

    protected virtual void OnNameChanged(string Value) => NameChanged?.Invoke(this, new EventArgs<string>(Value));

    protected virtual string OnPreviewNameChanged(string OldValue, string NewValue) => NewValue;

    public override void OnPropertyChanging(PropertyChangingEventArgs e)
    {
        base.OnPropertyChanging(e);
        if (e.PropertyName == nameof(Name))
            e.NewValue = OnPreviewNameChanged((string)e.OldValue, (string)e.NewValue);
    }

    public override void OnPropertyChanged(PropertyEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.PropertyName == nameof(Name))
            OnNameChanged(Name);
    }

    public override string ToString() => Name;
}

#endregion

#region Namable<T>

/// <inheritdoc/>
/// <remarks><see cref="Namable"/> with a generic value.</remarks>
[Serializable]
public class Namable<T> : Namable
{
    public virtual T Value { get => Get<T>(); set => Set(value); }

    public Namable() : base() { }

    public Namable(string name) : base(name) { }

    public Namable(string name, T value) : this(name) => Value = value;
}

#endregion
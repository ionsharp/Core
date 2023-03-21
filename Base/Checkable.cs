using Imagin.Core.Input;
using Imagin.Core.Linq;
using System;

namespace Imagin.Core;

#region Checkable

/// <summary>Specifies an <see cref="object"/> that implements <see cref="ICheck"/>.</summary>
public class Checkable : Base, ICheck
{
    public event EventHandler<EventArgs> Checked;
        
    public event CheckedEventHandler StateChanged;

    public event EventHandler<EventArgs> Unchecked;

    public virtual bool? IsChecked { get => Get<bool?>(null); set => Set(value); }

    public Checkable() : base() { }

    public Checkable(bool isChecked = false) => IsChecked = isChecked;

    public override void OnPropertyChanged(PropertyEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.PropertyName == nameof(IsChecked))
            IsChecked?.If(true, OnChecked, OnUnchecked);
    }

    public override string ToString() => IsChecked?.ToString() ?? "Indeterminate";

    protected virtual void OnChecked()
    {
        Checked?.Invoke(this, new EventArgs());
        OnStateChanged(true);
    }

    protected virtual void OnIndeterminate() => OnStateChanged(null);

    protected virtual void OnStateChanged(bool? State) => StateChanged?.Invoke(this, new CheckedEventArgs(State));

    protected virtual void OnUnchecked()
    {
        Unchecked?.Invoke(this, new EventArgs());
        OnStateChanged(false);
    }
}

#endregion

#region Checkable<T>

/// <inheritdoc/>
/// <summary>Specifies an <see cref="object"/> that implements <see cref="ICheck"/> (inherits <see cref="Checkable"/>).</summary>
public class BaseCheckable<T> : Checkable
{
    public T Value { get => Get<T>(); set => Set(value); }

    public override string ToString() => Value.ToString();

    public BaseCheckable() : this(false) { }

    public BaseCheckable(T value, bool isChecked = false) : this(isChecked) => Value = value;

    public BaseCheckable(bool isChecked = false) : base(isChecked) { }
}

#endregion
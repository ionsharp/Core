using System;

namespace Imagin.Core.Models;

/// <inheritdoc/>
[Serializable]
public class ViewModel<T> : ViewModel
{
    public virtual T View
    {
        get => Get<T>(default);
        set => Set(value);
    }

    protected ViewModel() : base() { }

    public ViewModel(T view) : this() => View = view;
}
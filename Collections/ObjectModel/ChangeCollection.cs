using System;

namespace Imagin.Core.Collections.ObjectModel;

[Serializable]
public class ChangeCollection<T> : ObservableCollection<T>, IChange
{
    public event ChangedEventHandler Changed;

    [Hide]
    public sealed override bool ObserveItems => true;

    public ChangeCollection() : base() { }

    protected virtual void OnChanged() => Changed?.Invoke(this);

    protected override void OnItemChanged(System.ComponentModel.PropertyChangedEventArgs e)
    {
        base.OnItemChanged(e);
        OnChanged();
    }

    public override void OnPropertyChanged(PropertyEventArgs e)
    {
        base.OnPropertyChanged(e);
        OnChanged();
    }
}
using Imagin.Core.Collections.ObjectModel;
using System;

namespace Imagin.Core.Data;

[Serializable]
public class SortDescription : Namable
{
    public SortDirection Direction { get => Get(SortDirection.Ascending); set => Set(value); }

    public SortDescription() : this(default, default) { }

    public SortDescription(string name, SortDirection direction) : base(name)
    {
        Direction = direction;
    }
}

[Serializable]
public class SortDescriptionCollection : ObservableCollection<SortDescription>
{
    public SortDescriptionCollection() : base() { }
}
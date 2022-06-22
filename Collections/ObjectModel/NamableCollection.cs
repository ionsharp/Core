using Imagin.Core.Collections.Generic;
using System;

namespace Imagin.Core.Collections.ObjectModel;

[Serializable]
public class NamableCollection<T> : ObservableCollection<T>
{
    string name = string.Empty;
    public string Name
    {
        get => name;
        set => this.Change(ref name, value);
    }

    public NamableCollection() : base() { }

    public NamableCollection(string name) : base() => Name = name;

    public override string ToString() => Name;
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagin.Core.Collections.ObjectModel;

[Serializable]
public class NamableCollection<T> : ObservableCollection<T>
{
    public string Name { get => this.Get(""); set => this.Set(value); }

    public NamableCollection() : this(default, default) { }

    public NamableCollection(string name) : this(name, default) { }

    public NamableCollection(string name, IEnumerable<T> items) : base(items ?? Enumerable.Empty<T>()) => Name = name;

    public override string ToString() => Name;
}
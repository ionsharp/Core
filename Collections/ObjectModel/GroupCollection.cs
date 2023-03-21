using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagin.Core.Collections.ObjectModel;

[Serializable]
public class GroupCollection<T> : NamableCollection<GroupItem<T>>, IGroup
{
    public GroupCollection() : base() { }

    public GroupCollection(string name) : base(name) { }

    public GroupCollection(string name, IEnumerable<T> items) : base(name, items.Select(i => new GroupItem<T>(i))) { }

    public GroupCollection(string name, IEnumerable<GroupItem<T>> items) : base(name, items) { }

    public void Add(string name, T item)
        => Add(new GroupItem<T>(name, item));

    public void Add(string name, string description, T item)
        => Add(new GroupItem<T>(name, description, item));
}
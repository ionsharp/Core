using System;
using System.Collections.Generic;

namespace Imagin.Core.Collections.ObjectModel;

[Serializable]
public class GroupCollection<T> : NamableCollection<T>, IGroup
{
    public GroupCollection() : this(default, default) { }

    public GroupCollection(string name) : this(name, default) { }

    public GroupCollection(string name, IEnumerable<T> items) : base(name, items) { }
}
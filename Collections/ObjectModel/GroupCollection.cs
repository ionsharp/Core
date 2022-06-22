using System;

namespace Imagin.Core.Collections.ObjectModel;

[Serializable]
public class GroupCollection<T> : NamableCollection<T>, IGroup
{
    public GroupCollection() : base() { }

    public GroupCollection(string name) : base(name) { }
}
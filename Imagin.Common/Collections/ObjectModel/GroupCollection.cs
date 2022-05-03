using System;

namespace Imagin.Common.Collections.Generic
{
    [Serializable]
    public class GroupCollection<T> : ObservableCollection<T>, IGroup
    {
        string name = string.Empty;
        public string Name
        {
            get => name;
            set => this.Change(ref name, value);
        }

        public GroupCollection() : base() { }

        public GroupCollection(string name) : base() => Name = name;

        public override string ToString() => Name;
    }
}
using Imagin.Common.Collections.Generic;
using System;

namespace Imagin.Common.Data
{
    [Serializable]
    public class SortDescription : BaseNamable
    {
        SortDirection direction;
        public SortDirection Direction
        {
            get => direction;
            set => this.Change(ref direction, value);
        }

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
}
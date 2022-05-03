using System;
using System.Collections.ObjectModel;

namespace Imagin.Common.Collections.ObjectModel
{
    [Serializable]
    public class ObjectCollection : ObservableCollection<object>
    {
        public ObjectCollection() : base() { }
    }
}
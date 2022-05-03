using System;
using System.Collections.ObjectModel;

namespace Imagin.Common.Collections.ObjectModel
{
    [Serializable]
    public class StringCollection : ObservableCollection<string>
    {
        public StringCollection() : base() { }
    }
}
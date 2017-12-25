using Imagin.Common;
using System.Collections.ObjectModel;

namespace Imagin.NET.Demo
{
    public class HierarchialObject : NamedObject
    {
        ObservableCollection<HierarchialObject> items = null;
        public ObservableCollection<HierarchialObject> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
                OnPropertyChanged("Items");
            }
        }

        public HierarchialObject(string Name) : base(Name)
        {
            Items = new ObservableCollection<HierarchialObject>();
        }
    }
}

using Imagin.Common.Collections;
using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Imagin.Common
{
    [Serializable]
    [XmlRoot(ElementName = "VirtualFolder")]
    public sealed class VirtualFolder : NamedObject
    {
        ConcurrentObservableCollection<AbstractObject> items = null;
        [XmlArray(ElementName = "Items")]
        public ConcurrentObservableCollection<AbstractObject> Items
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

        public VirtualFolder() : base()
        {
            this.Items = new ConcurrentObservableCollection<AbstractObject>();
        }

        public VirtualFolder(string Name) : base(Name)
        {
            this.Items = new ConcurrentObservableCollection<AbstractObject>();
        }
    }
}

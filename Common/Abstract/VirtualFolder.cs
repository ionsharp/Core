using Imagin.Common.Collections.Concurrent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Imagin.Common
{
    [Serializable]
    [XmlRoot(ElementName = "VirtualFolder")]
    public sealed class VirtualFolder : NamedObject, ICloneable, IContainer
    {
        AbstractObjectCollection items = null;
        [Category("General")]
        [Description("A collection of objects.")]
        [XmlArray(ElementName = "Items")]
        public AbstractObjectCollection Items
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

        public IEnumerable<AbstractObject> GetClones(VirtualFolder Folder)
        {
            foreach (var i in Folder.Items)
            {
                if (i is ICloneable)
                    yield return i;
            }
        }

        public object Clone()
        {
            var Result = new VirtualFolder();
            Result.Items = new AbstractObjectCollection(this.GetClones(this));
            return Result;
        }

        public VirtualFolder() : base()
        {
            this.Items = new AbstractObjectCollection();
        }

        public VirtualFolder(string Name) : base(Name)
        {
            this.Items = new AbstractObjectCollection();
        }
    }
}

using Imagin.Common.Attributes;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Imagin.Common
{
    [Serializable]
    public class NamedObject : AbstractObject, IEditable, INamable
    {
        protected string name = string.Empty;
        [Category("General")]
        [Featured(true)]
        [XmlElement(ElementName = "Name", Namespace = "")]
        public virtual string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        bool isEditable = false;
        [Hide(true)]
        [XmlIgnore]
        public bool IsEditable
        {
            get
            {
                return isEditable;
            }
            set
            {
                isEditable = value;
                OnPropertyChanged("IsEditable");
            }
        }

        public NamedObject() : base()
        {
        }

        public NamedObject(string Name) : base()
        {
            this.Name = Name;
        }

        public NamedObject(string Name, bool IsEditable) : base()
        {
            this.Name = Name;
            this.IsEditable = IsEditable;
        }
    }
}

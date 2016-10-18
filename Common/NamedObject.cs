using Imagin.Common.Attributes;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Imagin.Common
{
    [Serializable]
    public class NamedObject : AbstractObject, IEditable, INamable
    {
        [XmlIgnore]
        protected string name = string.Empty;
        [Category("General")]
        [Description("The name of the object.")]
        [Featured(true)]
        [XmlElement(ElementName = "Name")]
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
        [Browsable(false)]
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

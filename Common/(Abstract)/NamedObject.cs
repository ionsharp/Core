using Imagin.Common.Attributes;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Imagin.Common
{
    /// <summary>
    /// A named, abstract object.
    /// </summary>
    [Serializable]
    public class NamedObject : AbstractObject, IEditable, INamable
    {
        [XmlIgnore]
        protected string name = string.Empty;
        [Category("General")]
        [Description("The name of the object.")]
        [Featured(true)]
        public virtual string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = OnNameChanged(value);
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

        protected virtual string OnNameChanged(string Value)
        {
            return Value;
        }
    }
}

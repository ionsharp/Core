using Imagin.Common.Attributes;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Imagin.Common
{
    [Serializable]
    public class NamedNotifiableObject : NotifiableObject, INamable
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
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public NamedNotifiableObject() : base()
        {
        }

        public NamedNotifiableObject(bool IsNotifyEnabled) : base(IsNotifyEnabled)
        {
        }

        public NamedNotifiableObject(string Name, bool IsNotifyEnabled = false) : base(IsNotifyEnabled)
        {
            this.Name = Name;
        }

        public NamedNotifiableObject(string Name, double Interval, bool IsNotifyEnabled = false) : base(Interval, IsNotifyEnabled)
        {
            this.Name = Name;
        }
    }
}

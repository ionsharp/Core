using System;
using System.Xml.Serialization;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class NamedNotifiableObject : NotifiableObject, INamable
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        protected string name = string.Empty;
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="enabled"></param>
        public NamedNotifiableObject(double interval = DefaultInterval, bool enabled = false) : base(interval, enabled)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="interval"></param>
        /// <param name="enabled"></param>
        public NamedNotifiableObject(string name, double interval = DefaultInterval, bool enabled = false) : base(interval, enabled)
        {
            Name = name;
        }
    }
}

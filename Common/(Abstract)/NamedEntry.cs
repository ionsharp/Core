using System;
using System.Xml.Serialization;

namespace Imagin.Common
{
    ///<summary>
    /// A named object with a date that periodically notifies.
    /// </summary>
    [Serializable]
    public class NamedEntry : Entry, INamable
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
        public NamedEntry() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="interval"></param>
        /// <param name="enabled"></param>
        public NamedEntry(string name, double interval = DefaultInterval, bool enabled = false) : this(name, DateTime.UtcNow, interval, enabled)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <param name="interval"></param>
        /// <param name="enabled"></param>
        public NamedEntry(string name, DateTime date, double interval = DefaultInterval, bool enabled = false) : base(date, interval, enabled)
        {
            Name = name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return name;
        }
    }
}

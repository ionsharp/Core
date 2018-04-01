using System;
using System.Xml.Serialization;

namespace Imagin.Common
{
    ///<summary>
    /// Specifies an <see cref="Entry"/> with a name (implements <see cref="INamable"/>).
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
            get => name;
            set => SetValue(ref name, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public NamedEntry() : this(string.Empty, null, null, false) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <param name="interval"></param>
        /// <param name="enabled"></param>
        public NamedEntry(string name, DateTime? date, TimeSpan? interval = null, bool enabled = false) : base(date, interval, enabled) => Name = name;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => name;
    }
}

using System;
using System.Xml.Serialization;

namespace Imagin.Common
{
    /// <summary>
    /// A named, abstract object.
    /// </summary>
    [Serializable]
    public class NamedObject : AbstractObject, INamable
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
                name = OnPreviewNameChanged(name, value);
                OnPropertyChanged("Name");
                OnNameChanged(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public NamedObject() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public NamedObject(string name) : base()
        {
            Name = name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        /// <returns></returns>
        protected virtual string OnPreviewNameChanged(string OldValue, string NewValue)
        {
            return NewValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnNameChanged(string Value)
        {
        }
    }
}

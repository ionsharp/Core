using System;
using System.Xml.Serialization;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class TaggedObject : AbstractObject, ITaggable
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        protected object tag = null; 
        /// <summary>
        /// 
        /// </summary>
        public object Tag
        {
            get
            {
                return tag;
            }
            set
            {
                tag = value;
                OnPropertyChanged("Tag");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TaggedObject() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public TaggedObject(object tag) : base()
        {
            Tag = tag;
        }
    }
}

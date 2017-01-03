using System;

namespace Imagin.Common
{
    [Serializable]
    public class TaggedObject : AbstractObject, ITaggable
    {
        object tag = null; 
        public object Tag
        {
            get
            {
                return this.tag;
            }
            set
            {
                this.tag = value;
                OnPropertyChanged("Tag");
            }
        }

        public TaggedObject() : base()
        {
        }

        public TaggedObject(object Tag) : base()
        {
            this.Tag = Tag;
        }
    }
}

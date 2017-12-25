namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class TaggedObject : BindableObject, ITaggable
    {
        /// <summary>
        /// 
        /// </summary>
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

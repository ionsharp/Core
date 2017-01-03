namespace Imagin.Common
{
    public class NamedDependencyObject : AbstractDependencyObject, INamable
    {
        protected string name = string.Empty;
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

        public NamedDependencyObject() : base()
        {
        }

        public NamedDependencyObject(string Name) : base()
        {
            this.Name = Name;
        }
    }
}

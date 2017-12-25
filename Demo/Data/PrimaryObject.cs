using Imagin.Common;

namespace Imagin.NET.Demo
{
    public class PrimaryObject : NamedObject
    {
        bool boolean = false;
        public bool Boolean
        {
            get
            {
                return boolean;
            }
            set
            {
                boolean = value;
                OnPropertyChanged("Boolean");
            }
        }

        [Featured(true)]
        public override string Name
        {
            get
            {
                return base.Name;
            }

            set
            {
                base.Name = value;
            }
        }

        double _double = 0d;
        public double Double
        {
            get
            {
                return _double;
            }
            set
            {
                _double = value;
                OnPropertyChanged("Double");
            }
        }

        SecondaryObject child = new SecondaryObject();
        public SecondaryObject Child
        {
            get
            {
                return child;
            }
            set
            {
                SetValue(ref child, value, () => Child);
            }
        }

        public PrimaryObject() : base()
        {
        }
    }
}

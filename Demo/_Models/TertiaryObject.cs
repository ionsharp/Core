using Imagin.Common;
using System.Windows;

namespace Imagin.NET.Demo
{
    public class TertiaryObject : NamedObject
    {
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

        byte _byte = 0;
        public byte Byte
        {
            get
            {
                return _byte;
            }
            set
            {
                _byte = value;
                OnPropertyChanged("Byte");
            }
        }

        string normalString = "Default string";
        public string NormalString
        {
            get
            {
                return normalString;
            }
            set
            {
                normalString = value;
                OnPropertyChanged("NormalString");
                OnPropertyChanged("NormalStringWithNoSetter");
            }
        }

        short _short = 0;
        public short Short
        {
            get
            {
                return _short;
            }
            set
            {
                _short = value;
                OnPropertyChanged("Short");
            }
        }

        Size size = new Size(0, 0);
        public Size Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
                OnPropertyChanged("Size");
            }
        }

        public TertiaryObject() : base()
        {
        }
    }
}
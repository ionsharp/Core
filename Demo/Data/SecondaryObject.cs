using Imagin.Common;
using System.Windows;

namespace Imagin.NET.Demo
{
    public class SecondaryObject : NamedObject
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

        decimal _decimal = 0m;
        public decimal Decimal
        {
            get
            {
                return _decimal;
            }
            set
            {
                _decimal = value;
                OnPropertyChanged("Decimal");
            }
        }

        Visibility _enum = Visibility.Visible;
        public Visibility Enum
        {
            get
            {
                return _enum;
            }
            set
            {
                _enum = value;
                OnPropertyChanged("Enum");
            }
        }

        long _long = 0L;
        public long Long
        {
            get
            {
                return _long;
            }
            set
            {
                _long = value;
                OnPropertyChanged("Long");
            }
        }

        TertiaryObject child = new TertiaryObject();
        public TertiaryObject Child
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

        public SecondaryObject() : base()
        {
        }
    }
}

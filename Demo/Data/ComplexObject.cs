using Imagin.Common;
using Imagin.Common.Data;
using System;
using System.Net;
using System.Windows;

namespace Imagin.NET.Demo
{
    public class ComplexObject : NamedObject
    {
        bool boolean = false;
        [Imagin.Common.Category("Misc Types")]
        [System.ComponentModel.Description("Description for Boolean property.")]
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

        byte _byte = (byte)0;
        [Imagin.Common.Category("Numeric Types")]
        [System.ComponentModel.Description("Description for Byte property.")]
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

        PrimaryObject child = new PrimaryObject();
        [Imagin.Common.Category("Special Types")]
        [System.ComponentModel.Description("Description for Child property.")]
        public PrimaryObject Child
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

        DateTime dateTime = DateTime.Now;
        [Imagin.Common.Category("Misc Types")]
        [System.ComponentModel.Description("Description for DateTime property.")]
        public DateTime DateTime
        {
            get
            {
                return dateTime;
            }
            set
            {
                dateTime = value;
                OnPropertyChanged("DateTime");
            }
        }

        decimal _decimal = 0m;
        [Imagin.Common.Category("Numeric Types")]
        [System.ComponentModel.Description("Description for Decimal property.")]
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

        double _double = 0d;
        [Imagin.Common.Category("Numeric Types")]
        [System.ComponentModel.Description("Description for Double property.")]
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

        Visibility _enum = Visibility.Visible;
        [Imagin.Common.Category("Misc Types")]
        [System.ComponentModel.Description("Description for Enum property.")]
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

        Guid guid = Guid.NewGuid();
        [Imagin.Common.Category("Misc Types")]
        [System.ComponentModel.Description("Description for Guid property.")]
        public Guid Guid
        {
            get
            {
                return guid;
            }
            set
            {
                guid = value;
                OnPropertyChanged("Guid");
            }
        }

        int _int = 0;
        [Imagin.Common.Category("Numeric Types")]
        [System.ComponentModel.Description("Description for Int property.")]
        public int Int
        {
            get
            {
                return _int;
            }
            set
            {
                _int = value;
                OnPropertyChanged("Int");
            }
        }

        long _long = 0L;
        [Imagin.Common.Category("Numeric Types")]
        [System.ComponentModel.Description("Description for Long property.")]
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

        long longFileSize = 0L;
        [Imagin.Common.Category("Numeric Types")]
        [System.ComponentModel.Description("Description for LongFileSize property.")]
        [LongFormat(LongFormat.FileSize)]
        public long LongFileSize
        {
            get
            {
                return longFileSize;
            }
            private set
            {
                longFileSize = value;
                OnPropertyChanged("LongFileSize");
            }
        }

        NetworkCredential networkCredential = new NetworkCredential("UserName", "Password");
        [Imagin.Common.Category("String Types")]
        [System.ComponentModel.Description("Description for NetworkCredential property.")]
        public NetworkCredential NetworkCredential
        {
            get
            {
                return networkCredential;
            }
            set
            {
                networkCredential = value;
                OnPropertyChanged("NetworkCredential");
            }
        }

        Point? nullablePoint = null;
        [Imagin.Common.Category("Numeric Types")]
        [System.ComponentModel.Description("Description for NullablePoint property.")]
        public Point? NullablePoint
        {
            get
            {
                return nullablePoint;
            }
            set
            {
                nullablePoint = value;
                OnPropertyChanged("NullablePoint");
            }
        }

        Point point = new Point(0, 0);
        [Imagin.Common.Category("Numeric Types")]
        [System.ComponentModel.Description("Description for Point property.")]
        public Point Point
        {
            get
            {
                return point;
            }
            set
            {
                point = value;
                OnPropertyChanged("Point");
            }
        }

        short _short = (short)0;
        [Imagin.Common.Category("Numeric Types")]
        [System.ComponentModel.Description("Description for Short property.")]
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
        [Imagin.Common.Category("Numeric Types")]
        [System.ComponentModel.Description("Description for Size property.")]
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

        [Imagin.Common.Category("String Types")]
        [System.ComponentModel.Description("Description for NormalString property.")]
        public string NormalStringWithNoSetter
        {
            get
            {
                return normalString;
            }
        }

        string normalString = "Default string";
        [Imagin.Common.Category("String Types")]
        [System.ComponentModel.Description("Description for NormalString property.")]
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

        string filePathString = string.Empty;
        [Imagin.Common.Category("String Types")]
        [System.ComponentModel.Description("Description for FilePathString property.")]
        [StringFormat(StringFormat.FilePath)]
        public string FilePathString
        {
            get
            {
                return filePathString;
            }
            set
            {
                filePathString = value;
                OnPropertyChanged("FilePathString");
            }
        }

        string folderPathString = string.Empty;
        [Imagin.Common.Category("String Types")]
        [System.ComponentModel.Description("Description for FolderPathString property.")]
        [StringFormat(StringFormat.FolderPath)]
        public string FolderPathString
        {
            get
            {
                return folderPathString;
            }
            set
            {
                folderPathString = value;
                OnPropertyChanged("FolderPathString");
            }
        }

        string multilineString = string.Empty;
        [Imagin.Common.Category("String Types")]
        [System.ComponentModel.Description("Description for MultilineString property.")]
        [StringFormat(StringFormat.Multiline)]
        public string MultilineString
        {
            get
            {
                return multilineString;
            }
            set
            {
                multilineString = value;
                OnPropertyChanged("MultilineString");
            }
        }

        string passwordString = string.Empty;
        [Imagin.Common.Category("String Types")]
        [System.ComponentModel.Description("Description for PasswordString property.")]
        [StringFormat(StringFormat.Password)]
        public string PasswordString
        {
            get
            {
                return passwordString;
            }
            set
            {
                passwordString = value;
                OnPropertyChanged("PasswordString");
            }
        }

        string tokensString = "Red;Green;Blue;Yellow;Orange;Black;Purple;";
        [Imagin.Common.Category("String Types")]
        [System.ComponentModel.Description("Description for TokensString property.")]
        [StringFormat(StringFormat.Tokens)]
        public string TokensString
        {
            get
            {
                return tokensString;
            }
            set
            {
                tokensString = value;
                OnPropertyChanged("TokensString");
            }
        }

        Uri uri = new Uri("http://www.google.com");
        [Imagin.Common.Category("Misc Types")]
        [System.ComponentModel.Description("Description for Uri property.")]
        public Uri Uri
        {
            get
            {
                return uri;
            }
            set
            {
                uri = value;
                OnPropertyChanged("Uri");
            }
        }

        Version version = new Version();
        [Imagin.Common.Category("Misc Types")]
        [System.ComponentModel.Description("Description for Version property.")]
        public Version Version
        {
            get
            {
                return version;
            }
            set
            {
                version = value;
                OnPropertyChanged("Version");
            }
        }

        [Imagin.Common.Category("String Types")]
        [System.ComponentModel.Description("Description for Name property.")]
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

        public ComplexObject() : this(string.Empty)
        {
        }

        public ComplexObject(string Name) : base(Name)
        {
        }
    }
}

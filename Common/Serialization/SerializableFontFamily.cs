using System;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Imagin.Common.Serialization
{
    [Serializable]
    public class SerializableFontFamily : NamedObject
    {
        #region Properties

        public override string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                this.fontFamily = new FontFamily(value);
                OnPropertyChanged("Name");
                OnPropertyChanged("FontFamily");
            }
        }

        FontFamily fontFamily = default(FontFamily);
        [XmlIgnore]
        public FontFamily FontFamily
        {
            get
            {
                return this.fontFamily;
            }
            set
            {
                this.fontFamily = value;
                this.name = value.ToString();
                OnPropertyChanged("FontFamily");
                OnPropertyChanged("Name");
            }
        }

        #endregion

        #region SerializableFontFamily

        SerializableFontFamily()
        {
        }

        public SerializableFontFamily(string Name) : base(Name)
        {
        }

        #endregion
    }
}

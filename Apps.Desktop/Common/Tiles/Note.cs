using Imagin.Common;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Imagin.Apps.Desktop
{
    [DisplayName("Note")]
    [Serializable]
    public class NoteTile : Tile
    {
        string fontFamily = "Calibri";
        [DisplayName("FontFamily"), XmlIgnore]
        public FontFamily FontFamily
        {
            get => new(fontFamily);
            set => this.Change(ref fontFamily, value.Source);
        }

        double fontSize = 16.0;
        [DisplayName("FontSize")]
        public double FontSize
        {
            get => fontSize;
            set => this.Change(ref fontSize, value);
        }

        string text;
        [Hidden]
        public string Text
        {
            get => text;
            set => this.Change(ref text, value);
        }

        public NoteTile() : base() { }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Text):
                    OnChanged();
                    break;
            }
        }
    }
}
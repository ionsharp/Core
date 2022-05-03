using Imagin.Common.Media;
using System;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;

namespace Imagin.Common.Controls
{
    [DisplayName(nameof(Cell))]
    [Serializable]
    public class Cell : Base
    {
        enum Category
        {
            Alignment,
            Fill,
            Font,
            Format
        }

        Alignment alignment = new(HorizontalAlignment.Left, VerticalAlignment.Center);
        [Category(Category.Alignment)]
        public Alignment Alignment
        {
            get => alignment;
            set => this.Change(ref alignment, value);
        }

        StringColor fill = System.Windows.Media.Colors.Transparent;
        [Category(nameof(Category.Fill))]
        public StringColor Fill
        {
            get => fill;
            set => this.Change(ref fill, value);
        }

        StringColor fontColor = System.Windows.Media.Colors.Black;
        [Category(nameof(Category.Font))]
        [DisplayName("Color")]
        public StringColor FontColor
        {
            get => fontColor;
            set => this.Change(ref fontColor, value);
        }

        string fontFamily;
        [Category(Category.Font)]
        [DisplayName("Family")]
        public string FontFamily
        {
            get => fontFamily;
            set => this.Change(ref fontFamily, value);
        }

        double fontSize = 15;
        [Category(Category.Font)]
        [DisplayName("Size")]
        [Range(8.0, 72.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double FontSize
        {
            get => fontSize;
            set => this.Change(ref fontSize, value);
        }

        CellFormats format = CellFormats.None;
        [Category(Category.Format)]
        [DisplayName("Type")]
        [XmlAttribute]
        public CellFormats Format
        {
            get => format;
            set => this.Change(ref format, value);
        }

        string formatText = string.Empty;
        [Category(Category.Format)]
        [DisplayName("Text")]
        public string FormatText
        {
            get => formatText;
            set => this.Change(ref formatText, value);
        }

        string text = string.Empty;
        [Featured]
        public string Text
        {
            get => text;
            set => this.Change(ref text, value);
        }

        string trim = $"{TextTrimming.CharacterEllipsis}";
        [Category(Category.Alignment)]
        [XmlAttribute]
        public TextTrimming Trim
        {
            get => (TextTrimming)Enum.Parse(typeof(TextTrimming), trim);
            set => this.Change(ref trim, value.ToString());
        }

        string wrap = $"{TextWrapping.Wrap}";
        [Category(Category.Alignment)]
        [XmlAttribute]
        public TextWrapping Wrap
        {
            get => (TextWrapping)Enum.Parse(typeof(TextWrapping), wrap);
            set => this.Change(ref wrap, value.ToString());
        }

        public Cell() : base() { }

        public Cell(string text) : this() => Text = text;
    }
}
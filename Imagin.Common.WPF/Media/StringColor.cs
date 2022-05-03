using Imagin.Common.Linq;
using System;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Imagin.Common.Media
{
    [Serializable]
    public class StringColor : Base
    {
        //[field: NonSerialized]
        [XmlIgnore]
        public SolidColorBrush Brush
        {
            get => new(Value);
            set => Value = value.Color;
        }

        string value = $"255,0,0,0";
        [XmlAttribute("Value")]
        public string Source
        {
            get => value;
            set
            {
                this.Change(ref this.value, value);
                this.Changed(() => Brush);
                this.Changed(() => Value);
            }
        }

        [XmlIgnore]
        public Color Value
        {
            get => Convert(this);
            set
            {
                this.Change(ref this.value, Convert(value));
                this.Changed(() => Brush);
                this.Changed(() => Source);
            }
        }

        [XmlIgnore]
        public byte A => Value.A;

        [XmlIgnore]
        public byte R => Value.R;

        [XmlIgnore]
        public byte G => Value.G;

        [XmlIgnore]
        public byte B => Value.B;

        [XmlIgnore]
        public Argb Argb
        {
            get
            {
                var result = Value;
                return new Argb(result.A, result.R, result.G, result.B);
            }
        }

        public static string Convert(Color color) => Convert(color.A, color.R, color.G, color.B);

        public static string Convert(byte a, byte r, byte g, byte b) => $"{a},{r},{g},{b}";

        public static Color Convert(string input)
        {
            var result = input.Split(',');
            return Color.FromArgb(result[0].Byte(), result[1].Byte(), result[2].Byte(), result[3].Byte());
        }

        public static Color Convert(StringColor input) => Convert(input.value);

        public StringColor() : base() { }

        public StringColor(Color color) : this() => value = Convert(color);

        public StringColor(byte a, byte r, byte g, byte b) : this(Color.FromArgb(a, r, g, b)) { }

        public static implicit operator Color(StringColor right) => Convert(right);

        public static implicit operator StringColor(Color right) => new(right);

        public override string ToString() => value;
    }
}
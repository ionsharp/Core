using System;
using System.Xml.Serialization;

namespace Imagin.Common.Media
{
    [Serializable]
    public class GradientStep : Base
    {
        StringColor color = default;
        [XmlElement]
        public StringColor Color
        {
            get => color;
            set => this.Change(ref color, value);
        }

        double offset = 0;
        [XmlAttribute]
        public double Offset
        {
            get => offset;
            set => this.Change(ref offset, value);
        }

        public GradientStep() : base() { }

        public GradientStep(double offset, StringColor color) : this()
        {
            Offset = offset;
            Color = color;
        }
    }
}
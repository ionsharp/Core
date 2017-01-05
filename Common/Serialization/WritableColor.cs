using System;
using System.Windows.Media;
using System.Xml.Serialization;
using Imagin.Common.Extensions;

namespace Imagin.Common.Serialization
{
    /// <summary>
    /// Facilitates with serializing standard .NET colors.
    /// </summary>
    [Serializable]
    public class WritableColor : WritableObject<string>
    {
        public WritableColor()
        {
            Value = "00000000";
        }

        public WritableColor(string Hex) : base(Hex)
        {
        }

        public WritableColor(Color Color) : base(Color.ToHexWithAlpha())
        {
        }

        public WritableColor(SolidColorBrush SolidColorBrush) : base(SolidColorBrush.Color.ToHexWithAlpha())
        {
        }

        protected override string OnPreviewValueChanged(string Value)
        {
            return Value.StartsWith("#") ? Value.Substring(1) : Value;
        }
    }
}

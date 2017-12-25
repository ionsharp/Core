using System;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Imagin.Common.Linq
{
    public static class StringExtensions
    {
        public static Color ToColor(this string Value, bool WithAlpha = false)
        {
            if (!Value.IsNullOrEmpty())
            {
                Value = Value.Replace("#", string.Empty);

                var Max = WithAlpha ? 8 : 6;

                var Length = Value.Length;

                if (Length > Max)
                {
                    Value = Value.Substring(0, Max);
                }
                else if (Length == 3)
                {
                    Value = "{0}{1}{2}{3}".F(WithAlpha ? "FF" : "", new string(Value[0], 2), new string(Value[1], 2), new string(Value[2], 2));
                }
                else
                {
                    Value = "{0}{1}".F(new string('0', Max - Length), Value);
                    Value = WithAlpha ? "FF{0}".F(Value.Substring(2)) : Value;
                }

                var a = (byte)(Convert.ToUInt32(WithAlpha ? Value.Substring(0, 2) : "FF", 16));
                var r = (byte)(Convert.ToUInt32(Value.Substring(WithAlpha ? 2 : 0, 2), 16));
                var g = (byte)(Convert.ToUInt32(Value.Substring(WithAlpha ? 4 : 2, 2), 16));
                var b = (byte)(Convert.ToUInt32(Value.Substring(WithAlpha ? 6 : 4, 2), 16));

                return Color.FromArgb(a, r, g, b);
            }
            return default(Color);
        }

        public static SolidColorBrush ToSolidColorBrush(this string Value, bool WithAlpha = false)
        {
            return new SolidColorBrush(Value.ToColor(WithAlpha));
        }
    }
}

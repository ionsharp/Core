using Imagin.Common.Media;
using System;
using Windows.UI;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Gets distance to a color from given color.
        /// </summary>
        /// <param name="First"></param>
        /// <param name="Second"></param>
        /// <returns></returns>
        public static double DistanceFrom(this Color First, Color Second)
        {
            return Math.Sqrt(Math.Pow(First.R - Second.R, 2) + Math.Pow(First.G - Second.G, 2) + Math.Pow(First.B - Second.B, 2));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static AccentColor ToAccentColor(this Color Value)
        {
            var Key = Value.ToHex().ToLower();
            foreach (var i in EnumExtensions.GetValues<AccentColor>())
            {
                var k = i.GetAttribute<KeyAttribute>().Key.ToString();
                if (k == Key)
                    return i;
            }

            throw new ArgumentOutOfRangeException("Value cannot be converted to a valid accent color.");
        }

        /// <summary>
        /// Converts color to hex string without alpha component.
        /// </summary>
        public static string ToHex(this Color Color)
        {
            return Color.R.ToString("X2") + Color.G.ToString("X2") + Color.B.ToString("X2");
        }

        /// <summary>
        /// Converts color to hex string with alpha component.
        /// </summary>
        public static string ToHexWithAlpha(this Color Color)
        {
            return Color.A.ToString("X2") + Color.R.ToString("X2") + Color.G.ToString("X2") + Color.B.ToString("X2");
        }

        /// <summary>
        /// Creates new color with specified alpha component from specified color.
        /// </summary>
        public static Color WithAlpha(this Color Color, byte Alpha)
        {
            return Color.FromArgb(Alpha, Color.R, Color.G, Color.B);
        }
    }
}

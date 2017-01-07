using System.Windows.Media;

namespace Imagin.Common.Extensions
{
    public static class ColorExtensions
    {
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

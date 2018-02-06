using System;
using System.Windows.Media;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class ColorExtensions
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
        /// <param name="color"></param>
        /// <returns></returns>
        public static System.Drawing.Color ToDrawing(this Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
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

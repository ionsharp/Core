using Imagin.Common.Numbers;
using Imagin.Common.Media;
using System;
using System.Windows.Media;

namespace Imagin.Common.Linq
{
    public static partial class XColor
    {
        public static readonly ResourceKey ToolTipTemplateKey = new();

        #region System.Drawing

        public static Color Double(this System.Drawing.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        #endregion

        #region System.Windows.Media

        public static Color A(this Color color, byte a) 
            => Color.FromArgb(a, color.R, color.G, color.B);

        public static Color A(this Color color, Func<byte, byte> a) 
            => Color.FromArgb(a(color.A), color.R, color.G, color.B);

        public static Color R(this Color color, byte r)
            => Color.FromArgb(color.A, r, color.G, color.B);

        public static Color R(this Color color, Func<byte, byte> r)
            => Color.FromArgb(color.A, r(color.R), color.G, color.B);

        public static Color G(this Color color, byte g) 
            => Color.FromArgb(color.A, color.R, g, color.B);

        public static Color G(this Color color, Func<byte, byte> g)
            => Color.FromArgb(color.A, color.R, g(color.G), color.B);

        public static Color B(this Color color, byte b) 
            => Color.FromArgb(color.A, color.R, color.G, b);

        public static Color B(this Color color, Func<byte, byte> b)
            => Color.FromArgb(color.A, color.R, color.G, b(color.B));

        //...

        public static Color Decode(this int color)
        {
            var a = (byte)(color >> 24);
            //Prevent division by zero
            int ai = a;
            if (ai == 0)
                ai = 1;

            //Scale inverse alpha to use cheap integer mul bit shift
            ai = ((255 << 8) / ai);

            return Color.FromArgb(a,
                (byte)((((color >> 16) & 0xFF) * ai) >> 8),
                (byte)((((color >> 8) & 0xFF) * ai) >> 8),
                (byte)((((color & 0xFF) * ai) >> 8)));
        }

        public static int Encode(this Color color)
        {
            var col = 0;
            if (color.A != 0)
            {
                var a = color.A + 1;
                col = (color.A << 24)
                  | ((byte)((color.R * a) >> 8) << 16)
                  | ((byte)((color.G * a) >> 8) << 8)
                  | ((byte)((color.B * a) >> 8));
            }
            return col;
        }

        public static int Encode(this Color input, System.Windows.Media.PixelFormat format)
        {
            var result = 0;
            if (format == System.Windows.Media.PixelFormats.BlackWhite)
            {

            }
            else if (format == System.Windows.Media.PixelFormats.Cmyk32)
            {

            }
            else if (format == System.Windows.Media.PixelFormats.Rgba128Float)
            {
                result = input.A << 96;
                result |= input.B << 64;
                result |= input.G << 32;
                result |= input.R << 0;
            }
            else if (format == System.Windows.Media.PixelFormats.Rgba64)
            {
                result = input.A << 48;
                result |= input.B << 32;
                result |= input.G << 16;
                result |= input.R << 0;
            }
            else if (format == System.Windows.Media.PixelFormats.Bgra32)
            {
                result = input.A << 24;
                result |= input.R << 16;
                result |= input.G << 8;
                result |= input.B << 0;
            }
            else if (format == System.Windows.Media.PixelFormats.Gray32Float)
            {
            }
            else if (format == System.Windows.Media.PixelFormats.Gray16)
            {
            }
            else if (format == System.Windows.Media.PixelFormats.Gray8)
            {
            }
            return result;
        }

        //...

        /// <summary>
        /// Gets distance to a color from given color.
        /// </summary>
        /// <param name="First"></param>
        /// <param name="Second"></param>
        /// <returns></returns>
        public static double GetDistance(this Color First, Color Second)
        {
            return Math.Sqrt(Math.Pow(First.R - Second.R, 2) + Math.Pow(First.G - Second.G, 2) + Math.Pow(First.B - Second.B, 2));
        }

        /// <summary>
        /// Gets the hue-saturation-lightness (HSL) hue value, in degrees, for this <see cref="Color"/> structure.
        /// </summary>
        /// <returns>The hue, in degrees, of this <see cref="Color"/>. The hue is measured in degrees, ranging from 0.0 through 360.0, in HSL color space.</returns>
        public static double GetHue(this Color color)
            => color.Int32().GetHue();

        /// <summary>
        /// Gets the hue-saturation-lightness (HSL) saturation value for this <see cref="Color"/> structure.
        /// </summary>
        /// <returns>The saturation of this <see cref="Color"/>. The saturation ranges from 0.0 through 1.0, where 0.0 is grayscale and 1.0 is the most saturated.</returns>
        public static double GetSaturation(this Color color)
            => color.Int32().GetSaturation();

        /// <summary>
        /// Gets the hue-saturation-lightness (HSL) lightness value for this <see cref="Color"/> structure.
        /// </summary>
        /// <returns>The lightness of this <see cref="Color"/>. The lightness ranges from 0.0 through 1.0, where 0.0 represents black and 1.0 represents white.</returns>
        public static double GetBrightness(this Color color)
            => color.Int32().GetBrightness();

        //...

        public static System.Drawing.Color Int32(this Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static Hexadecimal Hexadecimal(this Color color, bool alpha = true)
            => new(color.R, color.G, color.B, alpha ? color.A : (byte)255);

        #endregion
    }
}
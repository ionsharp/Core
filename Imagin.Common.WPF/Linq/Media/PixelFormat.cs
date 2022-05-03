namespace Imagin.Common.Linq
{
    public static partial class XPixelFormat
    {
        #region Imagin.Common.Media

        public static System.Drawing.Imaging.PixelFormat Imaging(this Media.PixelFormat format)
        {
            return format switch
            {
                /*
                                case Drawing.PixelFormat.Bgr101010:
                                    return System.Drawing.Imaging.PixelFormat.;
                                case Drawing.PixelFormat.BlackWhite:
                                    return System.Drawing.Imaging.PixelFormat.;
                                case Drawing.PixelFormat.Cmyk32:
                                    return System.Drawing.Imaging.PixelFormat.;
                                case Drawing.PixelFormat.Gray2:
                                    return System.Drawing.Imaging.PixelFormat.;
                                case Drawing.PixelFormat.Gray32Float:
                                    return System.Drawing.Imaging.PixelFormat.;
                                case Drawing.PixelFormat.Gray4:
                                    return System.Drawing.Imaging.PixelFormat.;
                                case Drawing.PixelFormat.Gray8:
                                    return System.Drawing.Imaging.PixelFormat.;
                                case Drawing.PixelFormat.Indexed2:
                                    return System.Drawing.Imaging.PixelFormat.;
                                case Drawing.PixelFormat.Prgba128Float:
                                    return System.Drawing.Imaging.PixelFormat.;
                                case Drawing.PixelFormat.Rgb128Float:
                                    return System.Drawing.Imaging.PixelFormat.;
                                case Drawing.PixelFormat.Rgba128Float:
                                    return System.Drawing.Imaging.PixelFormat.;
                                */
                Media.PixelFormat.Bgr24 => System.Drawing.Imaging.PixelFormat.Format24bppRgb,
                Media.PixelFormat.Bgr32 => System.Drawing.Imaging.PixelFormat.Format32bppRgb,
                Media.PixelFormat.Bgr555 => System.Drawing.Imaging.PixelFormat.Format16bppRgb555,
                Media.PixelFormat.Bgr565 => System.Drawing.Imaging.PixelFormat.Format16bppRgb565,
                Media.PixelFormat.Bgra32 => System.Drawing.Imaging.PixelFormat.Format32bppArgb,
                Media.PixelFormat.Gray16 => System.Drawing.Imaging.PixelFormat.Format16bppGrayScale,
                Media.PixelFormat.Indexed1 => System.Drawing.Imaging.PixelFormat.Format1bppIndexed,
                Media.PixelFormat.Indexed4 => System.Drawing.Imaging.PixelFormat.Format4bppIndexed,
                Media.PixelFormat.Indexed8 => System.Drawing.Imaging.PixelFormat.Format8bppIndexed,
                Media.PixelFormat.Pbgra32 => System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
                Media.PixelFormat.Prgba64 => System.Drawing.Imaging.PixelFormat.Format64bppPArgb,
                Media.PixelFormat.Rgb24 => System.Drawing.Imaging.PixelFormat.Format24bppRgb,
                Media.PixelFormat.Rgb48 => System.Drawing.Imaging.PixelFormat.Format48bppRgb,
                Media.PixelFormat.Rgba64 => System.Drawing.Imaging.PixelFormat.Format64bppArgb,
                _ => default,
            };
        }

        public static System.Windows.Media.PixelFormat Convert(this Media.PixelFormat format)
        {
            return format switch
            {
                Media.PixelFormat.Bgr101010 => System.Windows.Media.PixelFormats.Bgr101010,
                Media.PixelFormat.Bgr24 => System.Windows.Media.PixelFormats.Bgr24,
                Media.PixelFormat.Bgr32 => System.Windows.Media.PixelFormats.Bgr32,
                Media.PixelFormat.Bgr555 => System.Windows.Media.PixelFormats.Bgr555,
                Media.PixelFormat.Bgr565 => System.Windows.Media.PixelFormats.Bgr565,
                Media.PixelFormat.Bgra32 => System.Windows.Media.PixelFormats.Bgra32,
                Media.PixelFormat.BlackWhite => System.Windows.Media.PixelFormats.BlackWhite,
                Media.PixelFormat.Cmyk32 => System.Windows.Media.PixelFormats.Cmyk32,
                Media.PixelFormat.Gray16 => System.Windows.Media.PixelFormats.Gray16,
                Media.PixelFormat.Gray2 => System.Windows.Media.PixelFormats.Gray2,
                Media.PixelFormat.Gray32Float => System.Windows.Media.PixelFormats.Gray32Float,
                Media.PixelFormat.Gray4 => System.Windows.Media.PixelFormats.Gray4,
                Media.PixelFormat.Gray8 => System.Windows.Media.PixelFormats.Gray8,
                Media.PixelFormat.Indexed1 => System.Windows.Media.PixelFormats.Indexed1,
                Media.PixelFormat.Indexed2 => System.Windows.Media.PixelFormats.Indexed2,
                Media.PixelFormat.Indexed4 => System.Windows.Media.PixelFormats.Indexed4,
                Media.PixelFormat.Indexed8 => System.Windows.Media.PixelFormats.Indexed8,
                Media.PixelFormat.Pbgra32 => System.Windows.Media.PixelFormats.Pbgra32,
                Media.PixelFormat.Prgba128Float => System.Windows.Media.PixelFormats.Prgba128Float,
                Media.PixelFormat.Prgba64 => System.Windows.Media.PixelFormats.Prgba64,
                Media.PixelFormat.Rgb24 => System.Windows.Media.PixelFormats.Rgb24,
                Media.PixelFormat.Rgb128Float => System.Windows.Media.PixelFormats.Rgb128Float,
                Media.PixelFormat.Rgb48 => System.Windows.Media.PixelFormats.Rgb48,
                Media.PixelFormat.Rgba128Float => System.Windows.Media.PixelFormats.Rgba128Float,
                Media.PixelFormat.Rgba64 => System.Windows.Media.PixelFormats.Rgba64,
                _ => default,
            };
        }

        #endregion

        #region System.Windows.Media

        public static Media.PixelFormat Convert(this System.Windows.Media.PixelFormat format)
        {
            if (format == System.Windows.Media.PixelFormats.Bgr101010)
                return Media.PixelFormat.Bgr101010;
            if (format == System.Windows.Media.PixelFormats.Bgr24)
                return Media.PixelFormat.Bgr24;
            if (format == System.Windows.Media.PixelFormats.Bgr32)
                return Media.PixelFormat.Bgr32;
            if (format == System.Windows.Media.PixelFormats.Bgr555)
                return Media.PixelFormat.Bgr555;
            if (format == System.Windows.Media.PixelFormats.Bgr565)
                return Media.PixelFormat.Bgr565;
            if (format == System.Windows.Media.PixelFormats.Bgra32)
                return Media.PixelFormat.Bgra32;
            if (format == System.Windows.Media.PixelFormats.BlackWhite)
                return Media.PixelFormat.BlackWhite;
            if (format == System.Windows.Media.PixelFormats.Cmyk32)
                return Media.PixelFormat.Cmyk32;
            if (format == System.Windows.Media.PixelFormats.Gray16)
                return Media.PixelFormat.Gray16;
            if (format == System.Windows.Media.PixelFormats.Gray2)
                return Media.PixelFormat.Gray2;
            if (format == System.Windows.Media.PixelFormats.Gray32Float)
                return Media.PixelFormat.Gray32Float;
            if (format == System.Windows.Media.PixelFormats.Gray4)
                return Media.PixelFormat.Gray4;
            if (format == System.Windows.Media.PixelFormats.Gray8)
                return Media.PixelFormat.Gray8;
            if (format == System.Windows.Media.PixelFormats.Indexed1)
                return Media.PixelFormat.Indexed1;
            if (format == System.Windows.Media.PixelFormats.Indexed2)
                return Media.PixelFormat.Indexed2;
            if (format == System.Windows.Media.PixelFormats.Indexed4)
                return Media.PixelFormat.Indexed4;
            if (format == System.Windows.Media.PixelFormats.Indexed8)
                return Media.PixelFormat.Indexed8;
            if (format == System.Windows.Media.PixelFormats.Pbgra32)
                return Media.PixelFormat.Pbgra32;
            if (format == System.Windows.Media.PixelFormats.Prgba128Float)
                return Media.PixelFormat.Prgba128Float;
            if (format == System.Windows.Media.PixelFormats.Prgba64)
                return Media.PixelFormat.Prgba64;
            if (format == System.Windows.Media.PixelFormats.Rgb24)
                return Media.PixelFormat.Rgb24;
            if (format == System.Windows.Media.PixelFormats.Rgb128Float)
                return Media.PixelFormat.Rgb128Float;
            if (format == System.Windows.Media.PixelFormats.Rgb48)
                return Media.PixelFormat.Rgb48;
            if (format == System.Windows.Media.PixelFormats.Rgba128Float)
                return Media.PixelFormat.Rgba128Float;
            if (format == System.Windows.Media.PixelFormats.Rgba64)
                return Media.PixelFormat.Rgba64;
            return default;
        }

        #endregion
    }
}
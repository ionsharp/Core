using System;

namespace Imagin.Common.Media
{
    [Serializable]
    public enum PixelFormat
    {
        /// <summary>
        /// Gets the pixel format that is best suited for the particular operation.
        /// </summary>
        Default,
        /// <summary>
        /// Gets the pixel format specifying a paletted bitmap with 2 colors.
        /// </summary>
        Indexed1,
        /// <summary>
        /// Gets the pixel format specifying a paletted bitmap with 4 colors.
        /// </summary>
        Indexed2,
        /// <summary>
        /// Gets the pixel format specifying a paletted bitmap with 16 colors.
        /// </summary>
        Indexed4,
        /// <summary>
        /// Gets the pixel format specifying a paletted bitmap with 256 colors.
        /// </summary>
        Indexed8,
        /// <summary>
        /// Gets the black and white pixel format which displays one bit of data per pixel
        /// as either black or white.
        /// </summary>
        BlackWhite,
        /// <summary>
        /// Gets the System.Windows.Media.PixelFormats.Gray2 pixel format which displays
        /// a 2 bits-per-pixel grayscale channel, allowing 4 shades of gray.
        /// </summary>
        Gray2,
        /// <summary>
        /// Gets the System.Windows.Media.PixelFormats.Gray4 pixel format which displays
        /// a 4 bits-per-pixel grayscale channel, allowing 16 shades of gray.
        /// </summary>
        Gray4,
        /// <summary>
        /// Gets the System.Windows.Media.PixelFormats.Gray8 pixel format which displays
        /// an 8 bits-per-pixel grayscale channel, allowing 256 shades of gray.
        /// </summary>
        Gray8,
        /// <summary>
        /// Gets the System.Windows.Media.PixelFormats.Bgr555 pixel format. System.Windows.Media.PixelFormats.Bgr555
        /// is a sRGB format with 16 bits per pixel (BPP). Each color channel (blue, green,
        /// and red) is allocated 5 bits per pixel (BPP).
        /// </summary>
        Bgr555,
        /// <summary>
        /// Gets the System.Windows.Media.PixelFormats.Bgr565 pixel format. System.Windows.Media.PixelFormats.Bgr565
        /// is a sRGB format with 16 bits per pixel (BPP). Each color channel (blue, green,
        /// and red) is allocated 5, 6, and 5 bits per pixel (BPP) respectively.
        /// </summary>
        Bgr565,
        /// <summary>
        /// Gets the System.Windows.Media.PixelFormats.Rgb128Float pixel format. System.Windows.Media.PixelFormats.Rgb128Float
        /// is a ScRGB format with 128 bits per pixel (BPP). Each color channel is allocated
        /// 32 BPP. This format has a gamma of 1.0.
        /// </summary>
        Rgb128Float,
        /// <summary>
        /// Gets the System.Windows.Media.PixelFormats.Bgr24 pixel format. System.Windows.Media.PixelFormats.Bgr24
        /// is a sRGB format with 24 bits per pixel (BPP). Each color channel (blue, green,
        /// and red) is allocated 8 bits per pixel (BPP).
        /// </summary>
        Bgr24,
        /// <summary>
        /// Gets the System.Windows.Media.PixelFormats.Rgb24 pixel format. System.Windows.Media.PixelFormats.Rgb24
        /// is a sRGB format with 24 bits per pixel (BPP). Each color channel (red, green,
        /// and blue) is allocated 8 bits per pixel (BPP).
        /// </summary>
        Rgb24,
        /// <summary>
        /// Gets the System.Windows.Media.PixelFormats.Bgr101010 pixel format. System.Windows.Media.PixelFormats.Bgr101010
        /// is a sRGB format with 32 bits per pixel (BPP). Each color channel (blue, green,
        /// and red) is allocated 10 bits per pixel (BPP).
        /// </summary>
        Bgr101010,
        /// <summary>
        /// Gets the System.Windows.Media.PixelFormats.Bgr32 pixel format. System.Windows.Media.PixelFormats.Bgr32
        /// is a sRGB format with 32 bits per pixel (BPP). Each color channel (blue, green,
        /// and red) is allocated 8 bits per pixel (BPP).
        /// </summary>
        Bgr32,
        /// <summary>
        /// Gets the System.Windows.Media.PixelFormats.Bgra32 pixel format. System.Windows.Media.PixelFormats.Bgra32
        /// is a sRGB format with 32 bits per pixel (BPP). Each channel (blue, green, red,
        /// and alpha) is allocated 8 bits per pixel (BPP).
        /// </summary>
        Bgra32,
        /// <summary>
        /// Gets the System.Windows.Media.PixelFormats.Pbgra32 pixel format. System.Windows.Media.PixelFormats.Pbgra32
        /// is a sRGB format with 32 bits per pixel (BPP). Each channel (blue, green, red,
        /// and alpha) is allocated 8 bits per pixel (BPP). Each color channel is pre-multiplied
        /// by the alpha value.
        /// </summary>
        Pbgra32,
        /// <summary>
        /// Gets the System.Windows.Media.PixelFormats.Rgb48 pixel format. System.Windows.Media.PixelFormats.Rgb48
        /// is a sRGB format with 48 bits per pixel (BPP). Each color channel (red, green,
        /// and blue) is allocated 16 bits per pixel (BPP). This format has a gamma of 1.0.
        /// </summary>
        Rgb48,
        /// <summary>
        /// Gets the System.Windows.Media.PixelFormats.Rgba64 pixel format. System.Windows.Media.PixelFormats.Rgba64
        /// is an sRGB format with 64 bits per pixel (BPP). Each channel (red, green, blue,
        /// and alpha) is allocated 16 bits per pixel (BPP). This format has a gamma of 1.0.
        /// </summary>
        Rgba64,
        /// <summary>
        /// Gets the System.Windows.Media.PixelFormats.Prgba64 pixel format. System.Windows.Media.PixelFormats.Prgba64
        /// is a sRGB format with 64 bits per pixel (BPP). Each channel (blue, green, red,
        /// and alpha) is allocated 32 bits per pixel (BPP). Each color channel is pre-multiplied
        /// by the alpha value. This format has a gamma of 1.0.
        /// </summary>
        Prgba64,
        /// <summary>
        /// Gets the System.Windows.Media.PixelFormats.Gray16 pixel format which displays
        /// a 16 bits-per-pixel grayscale channel, allowing 65536 shades of gray. This format
        /// has a gamma of 1.0.
        /// </summary>
        Gray16,
        /// <summary>
        /// Gets the System.Windows.Media.PixelFormats.Gray32Float pixel format. System.Windows.Media.PixelFormats.Gray32Float
        /// displays a 32 bits per pixel (BPP) grayscale channel, allowing over 4 billion
        /// shades of gray. This format has a gamma of 1.0.
        /// </summary>
        Gray32Float,
        /// <summary>
        /// Gets the System.Windows.Media.PixelFormats.Rgba128Float pixel format. System.Windows.Media.PixelFormats.Rgba128Float
        /// is a ScRGB format with 128 bits per pixel (BPP). Each color channel is allocated
        /// 32 bits per pixel (BPP). This format has a gamma of 1.0.
        /// </summary>
        Rgba128Float,
        /// <summary>
        /// Gets the System.Windows.Media.PixelFormats.Prgba128Float pixel format. System.Windows.Media.PixelFormats.Prgba128Float
        /// is a ScRGB format with 128 bits per pixel (BPP). Each channel (red, green, blue,
        /// and alpha) is allocated 32 bits per pixel (BPP). Each color channel is pre-multiplied
        /// by the alpha value. This format has a gamma of 1.0.
        /// </summary>
        Prgba128Float,
        /// <summary>
        /// Gets the System.Windows.Media.PixelFormats.Cmyk32 pixel format which displays
        /// 32 bits per pixel (BPP) with each color channel (cyan, magenta, yellow, and black)
        /// allocated 8 bits per pixel (BPP).
        /// </summary>
        Cmyk32, 
    }
}
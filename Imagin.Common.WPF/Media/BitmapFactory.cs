using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Imagin.Common.Linq;

namespace Imagin.Common.Media
{
    /// <summary>
    /// Cross-platform factory for <see cref="WriteableBitmap"/>.
    /// </summary>
    public static class BitmapFactory
    {
        /// <summary>
        /// Creates a new <see cref="WriteableBitmap"/> of the specified width and height
        /// </summary>
        /// <remarks>For WPF, the default DPI is 96x96 and PixelFormat is Pbgra32</remarks>
        public static WriteableBitmap New(int pixelWidth, int pixelHeight, Color color = default) => New(new System.Drawing.Size(pixelWidth, pixelHeight), color);

        /// <summary>
        /// Creates a new <see cref="WriteableBitmap"/> of the specified width and height
        /// </summary>
        /// <remarks>For WPF, the default DPI is 96x96 and PixelFormat is Pbgra32</remarks>
        public static WriteableBitmap New(System.Drawing.Size size, Color color = default)
        {
            var result = new WriteableBitmap(size.Width < 1 ? 1 : size.Width, size.Height < 1 ? 1 : size.Height, 96.0, 96.0, PixelFormats.Pbgra32, null);
            if (color != default)
                result.Clear(color);

            return result;
        }

        /// <summary>
        /// Creates a new <see cref="WriteableBitmap"/> from the given <see cref="ColorMatrix"/>.
        /// </summary>
        public static WriteableBitmap New(ColorMatrix input)
        {
            if (input != null)
            {
                var result = New(input.Columns.Int32(), input.Rows.Int32());
                input.Each((y, x, i) =>
                {
                    result.SetPixel(x, y, Color.FromArgb(i.A, i.R, i.G, i.B));
                    return i;
                });
                return result;
            }
            return null;
        }

        //...

        public static System.Drawing.Bitmap NewBitmap(System.Drawing.Size size) => NewBitmap(size.Height, size.Width);

        public static System.Drawing.Bitmap NewBitmap(int height, int width) => new(width, height);

        //...

        /// <summary>
        /// Converts the input BitmapSource to the Pbgra32 format WriteableBitmap which is internally used by the WriteableBitmapEx.
        /// </summary>
        /// <param name="source">The source bitmap.</param>
        /// <returns></returns>
        public static WriteableBitmap ConvertToPbgra32Format(BitmapSource source)
        {
            // Convert to Pbgra32 if it's a different format
            if (source.Format == PixelFormats.Pbgra32)
                return new WriteableBitmap(source);

            var formatedBitmapSource = new FormatConvertedBitmap();
            formatedBitmapSource.BeginInit();
            formatedBitmapSource.Source = source;
            formatedBitmapSource.DestinationFormat = PixelFormats.Pbgra32;
            formatedBitmapSource.EndInit();
            return new WriteableBitmap(formatedBitmapSource);
        }

        /// <summary>
        /// Loads an image from the applications resource file and returns a new WriteableBitmap.
        /// </summary>
        /// <param name="relativePath">Only the relative path to the resource file. The assembly name is retrieved automatically.</param>
        /// <returns>A new WriteableBitmap containing the pixel data.</returns>
        public static WriteableBitmap FromResource(string relativePath)
        {
            var fullName = Assembly.GetCallingAssembly().FullName;
            var asmName = new AssemblyName(fullName).Name;
            return FromContent(asmName + ";component/" + relativePath);
        }

        /// <summary>
        /// Loads an image from the applications content and returns a new WriteableBitmap.
        /// </summary>
        /// <param name="relativePath">Only the relative path to the content file.</param>
        /// <returns>A new WriteableBitmap containing the pixel data.</returns>
        public static WriteableBitmap FromContent(string relativePath)
        {
            using (var bmpStream = Application.GetResourceStream(new Uri(relativePath, UriKind.Relative)).Stream)
            {
                return FromStream(bmpStream);
            }
        }

        /// <summary>
        /// Loads the data from an image stream and returns a new WriteableBitmap.
        /// </summary>
        /// <param name="stream">The stream with the image data.</param>
        /// <returns>A new WriteableBitmap containing the pixel data.</returns>
        public static WriteableBitmap FromStream(Stream stream)
        {
            var bmpi = new BitmapImage();
            bmpi.BeginInit();
            bmpi.CreateOptions = BitmapCreateOptions.None;
            bmpi.StreamSource = stream;
            bmpi.EndInit();
            var bmp = new WriteableBitmap(bmpi);
            bmpi.UriSource = null;
            return bmp;
        }
    }
}
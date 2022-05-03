using Imagin.Common.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Imagin.Common.Linq
{
    public static class XBitmap
    {
        public static Bitmap Alpha(Bitmap input, float opacity)
        {
            var matrix = new ColorMatrix
            {
                Matrix33 = opacity
            };

            var attributes = new ImageAttributes();
            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            var image = new Bitmap(input.Width, input.Height);

            var g = Graphics.FromImage(image);
            g.DrawImage(input, new Rectangle(1, 1, input.Width - 2, input.Height - 2), 0, 0, input.Width, input.Height, GraphicsUnit.Pixel, attributes);

            return new Bitmap(input);
        }

        /// <summary>
        /// Converts this instance to a <see cref="System.Windows.Media.Imaging.BitmapImage"/>.
        /// </summary>
        /// <param name="Bitmap"></param>
        /// <returns></returns>
        public static BitmapImage BitmapImage(this Bitmap input)
        {
            if (input == null) return null;
            return input.BitmapSource().BitmapImage();
        }

        public static BitmapSource BitmapSource(this Bitmap input)
        {
            if (input == null) return null;
            IntPtr Pointer = input.GetHbitmap();
            BitmapSource Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(Pointer, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            Pointer.Dispose();
            return Source;
        }

        /// <summary>
        /// Converts this instance to a <see cref="byte"/> array.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static unsafe byte[] Bytes(this Bitmap input)
        {
            //Lock the bitmap's bits. 
            Rectangle rect = new(0, 0, input.Width, input.Height);
            System.Drawing.Imaging.BitmapData bmpData = input.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, input.PixelFormat);

            //Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            //Declare an array to hold the bytes of the bitmap.
            int ByteCount = bmpData.Stride * input.Height;
            byte[] Bytes = new byte[ByteCount];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, Bytes, 0, ByteCount);
            input.UnlockBits(bmpData);

            return Bytes;
        }

        #region public static System.Windows.Forms.Cursor Cursor(this Bitmap input, int hotX, int hotY)

        [DllImport("user32.dll")]
        static extern IntPtr CreateIconIndirect([In] ref ICONINFO piconinfo);

        [DllImport("user32.dll")]
        static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DestroyIcon(IntPtr hIcon);

        public static System.Windows.Forms.Cursor Cursor(this Bitmap input, int hotX, int hotY)
        {
            ICONINFO Info = new();
            IntPtr Handle = input.GetHicon();
            GetIconInfo(Handle, out Info);

            Info.xHotspot = hotX;
            Info.yHotspot = hotY;
            Info.fIcon = false;

            IntPtr h = CreateIconIndirect(ref Info);
            return new System.Windows.Forms.Cursor(h);
        }

        #endregion

        /// <summary>
        /// Converts this instance to a hexadecimal <see cref="string"/>.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Hexadecimal(this Bitmap input) => new System.Runtime.Remoting.Metadata.W3cXsd2001.SoapHexBinary(input.Bytes()).ToString();

        public static Bitmap PixelFormat(this Bitmap input, Media.PixelFormat format)
        {
            var result = new Bitmap(input.Width, input.Height, format.Imaging());

            using (var g = Graphics.FromImage(result))
                g.DrawImage(input, new Rectangle(0, 0, result.Width, result.Height));

            return result;
        }

        public static Bitmap Threshold(this Bitmap input, float value)
        {
            var result = new Bitmap(input.Width, input.Height);

            var attributes = new ImageAttributes();
            attributes.SetThreshold(value);

            var points = new System.Drawing.Point[]
            {
                new System.Drawing.Point(0, 0),
                new System.Drawing.Point(input.Width - 1, 0),
                new System.Drawing.Point(0, input.Height - 1),
            };

            var rect = new Rectangle(0, 0, input.Width, input.Height);

            using (var g = Graphics.FromImage(result))
                g.DrawImage(input, points, rect, GraphicsUnit.Pixel, attributes);

            return result;
        }

        public static int TotalColors(this Bitmap input)
        {
            var colors = new List<System.Windows.Media.Color>();
            var Bitmap = input.WriteableBitmap();

            Bitmap.ForEach((x, y, color) =>
            {
                if (!colors.Contains(color))
                    colors.Add(color);

                return color;
            });

            return colors.Count;
        }

        /// <summary>
        /// Converts this instance to a <see cref="System.Windows.Media.Imaging.WriteableBitmap"/>.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static WriteableBitmap WriteableBitmap(this Bitmap input)
        {
            var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(input.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            return new WriteableBitmap(bitmapSource);
        }
    }
}
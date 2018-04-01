using Imagin.Common.Media;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class WriteableBitmapExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="action"></param>
        public static void ForEach(this WriteableBitmap bitmap, Func<Pixel, Color> action)
        {
            bitmap.ForEach(-1, 0, bitmap.PixelWidth, 0, bitmap.PixelHeight, i => i + 1, i => i, action);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="start"></param>
        /// <param name="xstart"></param>
        /// <param name="xend"></param>
        /// <param name="ystart"></param>
        /// <param name="yend"></param>
        /// <param name="xincrement"></param>
        /// <param name="yincrement"></param>
        /// <param name="action"></param>
        /// <param name="xpre"></param>
        /// <param name="xpost"></param>
        /// <param name="ypre"></param>
        /// <param name="ypost"></param>
        public static void ForEach
        (
            this WriteableBitmap bitmap, 
            int start, 
            int xstart,
            int xend, 
            int ystart, 
            int yend, 
            Func<int, int> xincrement,
            Func<int, int> yincrement,
            Func<Pixel, Color> action, 
            Action<int> xpre = default(Action<int>),
            Action<int> xpost = default(Action<int>), 
            Action<int> ypre = default(Action<int>), 
            Action<int> ypost = default(Action<int>)
        )
        {
            unsafe
            {
                bitmap.Lock();
                var CurrentPixel = start;
                byte* Start = (byte*)(void*)bitmap.BackBuffer;

                for (var row = ystart; row < yend; row++)
                {
                    ypre?.Invoke(row);
                    for (var column = xstart; column < xend; column++)
                    {
                        xpre?.Invoke(column);
                        CurrentPixel = xincrement(CurrentPixel);
                        var color = default(Color);

                        if (bitmap.Format == System.Windows.Media.PixelFormats.Bgr24)
                        {
                            color = Color.FromArgb
                            (
                                255,
                                *(Start + CurrentPixel * 3 + 2),
                                *(Start + CurrentPixel * 3 + 1),
                                *(Start + CurrentPixel * 3 + 0)
                            );
                        }
                        else if (bitmap.Format == System.Windows.Media.PixelFormats.Bgra32)
                        {
                            color = Color.FromArgb
                            (
                                *(Start + CurrentPixel * 4 + 3),
                                *(Start + CurrentPixel * 4 + 2),
                                *(Start + CurrentPixel * 4 + 1),
                                *(Start + CurrentPixel * 4 + 0)
                            );
                        }

                        if (action != null)
                        {
                            var ncolor = action(new Pixel(color, column, row));

                            if (ncolor != color)
                            {
                                if (bitmap.Format == System.Windows.Media.PixelFormats.Bgr24)
                                {
                                    *(Start + CurrentPixel * 3 + 0) = ncolor.B;
                                    *(Start + CurrentPixel * 3 + 1) = ncolor.G;
                                    *(Start + CurrentPixel * 3 + 2) = ncolor.R;
                                }
                                else if (bitmap.Format == System.Windows.Media.PixelFormats.Bgra32)
                                {
                                    *(Start + CurrentPixel * 4 + 0) = ncolor.B;
                                    *(Start + CurrentPixel * 4 + 1) = ncolor.G;
                                    *(Start + CurrentPixel * 4 + 2) = ncolor.R;
                                    *(Start + CurrentPixel * 4 + 3) = ncolor.A;
                                }
                            }
                        }
                        xpost?.Invoke(column);
                    }
                    CurrentPixel = yincrement(CurrentPixel);
                    ypost?.Invoke(row);
                }

                bitmap.AddDirtyRect(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));
                bitmap.Unlock();
            }
        }
    }
}

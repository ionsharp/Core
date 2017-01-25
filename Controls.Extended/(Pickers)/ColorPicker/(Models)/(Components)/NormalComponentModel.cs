using Imagin.Common;
using Imagin.Common.Extensions;
using Imagin.Controls.Extended.Primitives;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Controls.Extended
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class NormalComponentModel : ComponentModel
    {
        /// <summary>
        /// Is the Normal bitmap independent of the specific color (false for all but Hue of HSB)
        /// </summary>
        public virtual bool IsNormalIndependantOfColor
        {
            get
            {
                return false;
            }
        }

        bool isEnabled = false;
        /// <summary>
        /// 
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        /// <summary>
        /// Gets the color corresponding to a selected point (with 255 alpha)
        /// </summary>
        public abstract Color ColorAtPoint(Point selectionPoint, int colorComponentValue);

        /// <summary>
        /// Gets the point on the color plane that corresponds to the color (alpha ignored)
        /// </summary>
        public abstract Point PointFromColor(Color color);

        /// <summary>
        /// Updates the plane bitmap.
        /// </summary>
        public virtual void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
        {
            unsafe
            {
                Bitmap.Lock();

                int CurrentPixel = -1;
                byte* Start = (byte*)(void*)Bitmap.BackBuffer;

                var u = Unit.Value;
                u.Row = u.Row / 256.0;
                u.Column = u.Column / 256.0;

                double CurrentRow = u.Row * 256.0;

                for (int Row = 0; Row < Bitmap.PixelHeight; Row++)
                {
                    double CurrentCol = 0;
                    for (int Col = 0; Col < Bitmap.PixelWidth; Col++)
                    {
                        var c = Action.Invoke(new RowColumn(CurrentRow, CurrentCol), ComponentValue);

                        CurrentPixel++;
                        *(Start + CurrentPixel * 3 + 0) = c.B;
                        *(Start + CurrentPixel * 3 + 1) = c.G;
                        *(Start + CurrentPixel * 3 + 2) = c.R;
                        CurrentCol += u.Column;
                    }
                    CurrentRow -= u.Row;
                }
                Bitmap.AddDirtyRect(new Int32Rect(0, 0, Bitmap.PixelWidth, Bitmap.PixelHeight));
                Bitmap.Unlock();
            }
        }

        /// <summary>
        /// Updates the slider bitmap.
        /// </summary>
        public virtual void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false)
        {
            unsafe
            {
                Bitmap.Lock();
                int CurrentPixel = -1;
                byte* Start = (byte*)(void*)Bitmap.BackBuffer;

                double RowUnit = (Maximum.ToDouble() - Minimum.ToDouble()) / 256.0;
                double CurrentRow = Reverse ? Minimum.ToDouble() : Maximum.ToDouble();

                for (int Row = 0; Row < Bitmap.PixelHeight; Row++)
                {
                    var Rgba = Action.Invoke(Color, CurrentRow);
                    for (int Col = 0; Col < Bitmap.PixelWidth; Col++)
                    {
                        CurrentPixel++;
                        *(Start + CurrentPixel * 3 + 0) = Rgba.B;
                        *(Start + CurrentPixel * 3 + 1) = Rgba.G;
                        *(Start + CurrentPixel * 3 + 2) = Rgba.R;
                    }
                    if (Reverse) CurrentRow += RowUnit;
                    else CurrentRow -= RowUnit;
                }
                Bitmap.AddDirtyRect(new Int32Rect(0, 0, Bitmap.PixelWidth, Bitmap.PixelHeight));
                Bitmap.Unlock();
            }
        }
    }
}

using Imagin.Common;
using Imagin.Common.Drawing;
using Imagin.Common.Input;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SelectableComponentModel : ComponentModel, ISelectable
    {
        /// <summary>
        /// 
        /// </summary>
        public event SelectedEventHandler Selected;

        bool isSelected = false;
        /// <summary>
        /// 
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
                if (value) OnSelected();
            }
        }

        /// <summary>
        /// Gets whether or not the component is uniform.
        /// </summary>
        public virtual bool IsUniform
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ColorSpace"></param>
        public SelectableComponentModel(ColorSpaceModel ColorSpace) : base(ColorSpace)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnSelected()
        {
            Selected?.Invoke(this, new SelectedEventArgs(this));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected Point PointFromColor(double x, double y)
        {
            return new Point(x * 255d, 255 - (y * 255d));
        }

        /// <summary>
        /// Gets the color corresponding to a selected point (with 255 alpha)
        /// </summary>
        public abstract Color ColorAtPoint(Point selectionPoint, int colorComponentValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="newComponentValue"></param>
        /// <returns></returns>
        public abstract Color ColorFrom(Color color, int newComponentValue);

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

                var CurrentRow = u.Row * 256.0;

                for (var Row = 0; Row < Bitmap.PixelHeight; Row++)
                {
                    double CurrentCol = 0;
                    for (var Col = 0; Col < Bitmap.PixelWidth; Col++)
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
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="color"></param>
        /// <param name="orientation"></param>
        public void UpdateSlider(WriteableBitmap bitmap, Color color, Orientation orientation)
        {
            UpdateSlider(bitmap, color, null, false, orientation);
        }

        /// <summary>
        /// Updates the slider bitmap.
        /// </summary>
        public virtual void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false, Orientation Orientation = Orientation.Vertical)
        {
            unsafe
            {
                Bitmap.Lock();
                int CurrentPixel = -1;
                byte* Start = (byte*)(void*)Bitmap.BackBuffer;


                switch (Orientation)
                {
                    case Orientation.Horizontal:
                        var RowUnit = (Maximum - Minimum) / Bitmap.PixelHeight;
                        var CurrentRow = Reverse ? Minimum : Maximum;

                        for (var Row = 0; Row < Bitmap.PixelHeight; Row++)
                        {
                            for (var Col = 0; Col < Bitmap.PixelWidth; Col++)
                            {
                                var Rgba = Action.Invoke(Color, Col);

                                CurrentPixel++;
                                *(Start + CurrentPixel * 3 + 0) = Rgba.B;
                                *(Start + CurrentPixel * 3 + 1) = Rgba.G;
                                *(Start + CurrentPixel * 3 + 2) = Rgba.R;
                            }
                            if (Reverse) CurrentRow += RowUnit;
                            else CurrentRow -= RowUnit;
                        }
                        break;
                    case Orientation.Vertical:
                        RowUnit = (Maximum - Minimum) / Bitmap.PixelHeight;
                        CurrentRow = Reverse ? Minimum : Maximum;

                        for (var Row = 0; Row < Bitmap.PixelHeight; Row++)
                        {
                            var Rgba = Action.Invoke(Color, CurrentRow);
                            for (var Col = 0; Col < Bitmap.PixelWidth; Col++)
                            {
                                CurrentPixel++;
                                *(Start + CurrentPixel * 3 + 0) = Rgba.B;
                                *(Start + CurrentPixel * 3 + 1) = Rgba.G;
                                *(Start + CurrentPixel * 3 + 2) = Rgba.R;
                            }
                            if (Reverse) CurrentRow += RowUnit;
                            else CurrentRow -= RowUnit;
                        }
                        break;
                }
                Bitmap.AddDirtyRect(new Int32Rect(0, 0, Bitmap.PixelWidth, Bitmap.PixelHeight));
                Bitmap.Unlock();
            }
        }
    }
}

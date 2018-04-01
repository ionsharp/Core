using Imagin.Colour.Controls.Linq;
using Imagin.Colour.Linq;
using Imagin.Colour.Primitives;
using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class VisualComponent : Component, ISelectable
    {
        /// <summary>
        /// 
        /// </summary>
        public event SelectedEventHandler Selected;

        /// <summary>
        /// 
        /// </summary>
        public sealed override bool CanSelect => true;

        /// <summary>
        /// 
        /// </summary>
        public virtual ComponentKind Kind => ComponentKind.Default;

        bool isSelected = false;
        /// <summary>
        /// 
        /// </summary>
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                Property.Set(this, ref isSelected, value);
                if (value) OnSelected();
            }
        }

        /// <summary>
        /// Gets whether or not the component is uniform.
        /// </summary>
        public bool IsUniform => Kind == ComponentKind.Uniform;

        //-------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected System.Windows.Point PointFromColor(double x, double y) => new System.Windows.Point(x * 255.0, 255.0 - (y * 255.0));

        //-------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract Color ColorFrom(Color color, double value);

        /// <summary>
        /// Gets the color corresponding to a selected point (with 255 alpha).
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public abstract Color ColorFromPoint(System.Windows.Point point);

        /// <summary>
        /// Gets the point on the color plane that corresponds to the color (alpha ignored)
        /// </summary>
        public abstract System.Windows.Point PointFromColor(Color color);

        //-------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="value"></param>
        public abstract void DrawXY(WriteableBitmap bitmap, int value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="color"></param>
        /// <param name="orientation"></param>
        public abstract void DrawZ(WriteableBitmap bitmap, Color color, Orientation orientation = Orientation.Vertical);

        //-------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnSelected() => Selected?.Invoke(this, new SelectedEventArgs(this));

        //-------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="value"></param>
        /// <param name="action"></param>
        /// <param name="position"></param>
        protected void DrawXY(WriteableBitmap bitmap, int value, Func<System.Windows.Point, RGB> action, System.Windows.Point position)
        {
            position.X /= bitmap.PixelWidth.ToDouble();
            position.Y /= bitmap.PixelHeight.ToDouble();

            var CurrentColumn = 0.0;
            var CurrentRow = position.Y * bitmap.PixelHeight.ToDouble();

            bitmap.ForEach
            (
               -1, 0, bitmap.PixelWidth, 0, bitmap.PixelHeight,
                i => i + 1,
                i => i,
                pixel =>
                {
                    var color = action.Invoke(new System.Windows.Point(CurrentColumn, CurrentRow));
                    return System.Drawing.Color.FromArgb(255, (color.R * 255.0).ToByte(), (color.G * 255.0).ToByte(), (color.B * 255.0).ToByte());
                },
                null,
                x => CurrentColumn += position.X,
                y => CurrentColumn = 0,
                y => CurrentRow -= position.Y
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="action"></param>
        /// <param name="orientation"></param>
        protected void DrawZ(WriteableBitmap bitmap, Func<double, RGB> action, Orientation orientation)
        {
            var color       = default(Color);
            var current     = 0.0;
            var increment   = Maximum - Minimum;

            var reset   = new Action<int>(i => current  = Maximum);
            var proceed = new Action<int>(i => current -= increment);
            var obtain  = new Action<int>(i =>   color  = action.Invoke(current).ToColor());

            Action<int> xpre    = null, ypre = null;
            Action<int> xpost   = null, ypost = null;

            reset(0);
            switch (orientation)
            {
                case Orientation.Horizontal:
                    increment /= bitmap.PixelWidth;
                    xpre = obtain;
                    xpost = proceed;
                    ypost = reset;
                    break;
                case Orientation.Vertical:
                    increment /= bitmap.PixelHeight;
                    ypre = obtain;
                    ypost = proceed;
                    break;
            }

            bitmap.ForEach
            (
               -1, 0, bitmap.PixelWidth, 0, bitmap.PixelHeight,
                i => i + 1,
                i => i,
                pixel => color.ToDrawing(),
                xpre,
                xpost,
                ypre,
                ypost
            );
        }
    }
}
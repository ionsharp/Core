using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class ResizeAdorner : Adorner, ISubscribe, IUnsubscribe
    {
        #region Properties

        readonly Thumb top, bottom, left, right, topLeft, topRight, bottomLeft, bottomRight;

        readonly VisualCollection Children;

        protected override int VisualChildrenCount 
            => Children.Count;

        public Axis2D? CoerceAxis 
            => XElement.GetResizeCoerceAxis(AdornedElement as FrameworkElement);

        public CardinalDirection? CoerceDirection 
            => XElement.GetResizeCoerceDirection(AdornedElement as FrameworkElement);

        public double Snap 
            => XElement.GetResizeSnap(AdornedElement as FrameworkElement);

        public Style ThumbStyle 
            => XElement.GetResizeThumbStyle(AdornedElement as FrameworkElement);

        #endregion

        #region ResizeAdorner

        public ResizeAdorner(FrameworkElement element) : base(element)
        {
            Children
                = new VisualCollection(this);

            BuildThumb(ref top,
                Cursors.SizeNS);
            BuildThumb(ref left,
                Cursors.SizeWE);
            BuildThumb(ref right,
                Cursors.SizeWE);
            BuildThumb(ref bottom,
                Cursors.SizeNS);
            BuildThumb(ref topLeft,
                Cursors.SizeNWSE);
            BuildThumb(ref topRight,
                Cursors.SizeNESW);
            BuildThumb(ref bottomLeft,
                Cursors.SizeNESW);
            BuildThumb(ref bottomRight,
                Cursors.SizeNWSE);
        }

        #endregion

        #region Methods

        bool CanHandle(Thumb Thumb)
        {
            var result = true;

            if (AdornedElement == null || Thumb == null)
                result = false;

            if (result)
                EnforceSize(AdornedElement as FrameworkElement);

            return result;
        }

        //...
        
        /// <summary>
        /// Handler for resizing from the bottom-right.
        /// </summary>
        void HandleBottomRight(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            var target = AdornedElement as FrameworkElement;
            if (CanHandle(thumb))
            {
                if (CoerceAxis != Axis2D.Y)
                    target.Width = Math.Max(target.Width + e.HorizontalChange, thumb.DesiredSize.Width).NearestFactor(Snap);

                if (CoerceAxis != Axis2D.X)
                    target.Height = Math.Max(e.VerticalChange + target.Height, thumb.DesiredSize.Height).NearestFactor(Snap);
            }
        }

        /// <summary>
        /// Handler for resizing from the top-right.
        /// </summary>
        void HandleTopRight(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            var target = AdornedElement as FrameworkElement;
            if (CanHandle(thumb))
            {
                if (CoerceAxis != Axis2D.Y)
                    target.Width = Math.Max(target.Width + e.HorizontalChange, thumb.DesiredSize.Width).NearestFactor(Snap);

                if (CoerceAxis != Axis2D.X)
                {
                    var height_old = target.Height;
                    var height_new = Math.Max(target.Height - e.VerticalChange, thumb.DesiredSize.Height).NearestFactor(Snap);
                    var top_old = Canvas.GetTop(target);

                    target.Height = height_new;
                    Canvas.SetTop(target, top_old - (height_new - height_old));
                }
            }
        }

        /// <summary>
        /// Handler for resizing from the top-left.
        /// </summary>
        void HandleTopLeft(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            var target = AdornedElement as FrameworkElement;
            if (CanHandle(thumb))
            {
                if (CoerceAxis != Axis2D.Y)
                {
                    var width_old = target.Width;
                    var width_new = Math.Max(target.Width - e.HorizontalChange, thumb.DesiredSize.Width).NearestFactor(Snap);
                    var left_old = Canvas.GetLeft(target);

                    target.Width = width_new;
                    Canvas.SetLeft(target, left_old - (width_new - width_old));
                }
                if (CoerceAxis != Axis2D.X)
                {
                    var height_old = target.Height;
                    var height_new = Math.Max(target.Height - e.VerticalChange, thumb.DesiredSize.Height).NearestFactor(Snap);
                    var top_old = Canvas.GetTop(target);

                    target.Height = height_new;
                    Canvas.SetTop(target, top_old - (height_new - height_old));
                }
            }
        }

        /// <summary>
        /// Handler for resizing from the bottom-left.
        /// </summary>
        void HandleBottomLeft(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            var target = AdornedElement as FrameworkElement;
            if (CanHandle(thumb))
            {
                if (CoerceAxis != Axis2D.X)
                    target.Height = Math.Max(e.VerticalChange + target.Height, thumb.DesiredSize.Height).NearestFactor(Snap);

                if (CoerceAxis != Axis2D.Y)
                {
                    var width_old = target.Width;
                    var width_new = Math.Max(target.Width - e.HorizontalChange, thumb.DesiredSize.Width).NearestFactor(Snap);
                    var left_old = Canvas.GetLeft(target);

                    target.Width = width_new;
                    Canvas.SetLeft(target, left_old - (width_new - width_old));
                }
            }
        }

        /// <summary>
        /// Handler for resizing from the top.
        /// </summary>
        void HandleTop(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            var target = AdornedElement as FrameworkElement;
            if (CanHandle(thumb))
            {
                if (CoerceAxis != Axis2D.X)
                {
                    var height_old = target.Height;
                    var height_new = Math.Max(target.Height - e.VerticalChange, thumb.DesiredSize.Height).NearestFactor(Snap);
                    var top_old = Canvas.GetTop(target);
                    var top_new = top_old - (height_new - height_old);

                    target.Height = height_new;
                    Canvas.SetTop(target, top_new);
                }
            }
        }

        /// <summary>
        /// Handler for resizing from the left.
        /// </summary>
        void HandleLeft(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            var target = AdornedElement as FrameworkElement;
            if (CanHandle(thumb))
            {
                if (CoerceAxis != Axis2D.Y)
                {
                    var width_old = target.Width;
                    var width_new = Math.Max(target.Width - e.HorizontalChange, thumb.DesiredSize.Width).NearestFactor(Snap);
                    var left_old = Canvas.GetLeft(target);
                    var left_new = left_old - (width_new - width_old);

                    target.Width = width_new;
                    Canvas.SetLeft(target, left_new);
                }
            }
        }

        /// <summary>
        /// Handler for resizing from the right.
        /// </summary>
        void HandleRight(object sender, DragDeltaEventArgs e)
        {

            var thumb = sender as Thumb;
            var target = AdornedElement as FrameworkElement;
            if (CanHandle(thumb))
            {
                if (CoerceAxis != Axis2D.Y)
                {
                    var width = Math.Max(target.Width + e.HorizontalChange, thumb.DesiredSize.Width);
                    target.Width = width.NearestFactor(Snap);
                }
            }
        }

        /// <summary>
        /// Handler for resizing from the bottom.
        /// </summary>
        void HandleBottom(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            var target = AdornedElement as FrameworkElement;
            if (CanHandle(thumb))
            {
                if (CoerceAxis != Axis2D.X)
                {
                    var height = Math.Max(e.VerticalChange + target.Height, thumb.DesiredSize.Height);
                    target.Height = height.NearestFactor(Snap);
                }
            }
        }

        //...

        /// <summary>
        /// Helper method to instantiate the corner Thumbs, 
        /// set the Cursor property, set some appearance properties, 
        /// and add the elements to the visual tree.
        /// </summary>
        void BuildThumb(ref Thumb thumb, Cursor cursor)
        {
            if (thumb == null)
            {
                thumb = new Thumb()
                {
                    Background = Brushes.Black,
                    BorderThickness = new Thickness(0),
                    Cursor = cursor,
                    Height = 10,
                    Style = ThumbStyle,
                    Width = 10,
                };
                Children.Add(thumb);
            }
        }

        /// <summary>
        /// This method ensures that the Widths and Heights 
        /// are initialized.  Sizing to content produces Width 
        /// and Height values of Double.NaN.  Because this Adorner 
        /// explicitly resizes, the Width and Height need to be 
        /// set first.  It also sets the maximum size of the 
        /// adorned element.
        /// </summary>
        void EnforceSize(FrameworkElement input)
        {
            if (input.Width.Equals(double.NaN))
                input.Width = input.DesiredSize.Width;

            if (input.Height.Equals(double.NaN))
                input.Height = input.DesiredSize.Height;

            if (input.Parent is FrameworkElement parent)
            {
                input.MaxHeight 
                    = parent.ActualHeight;
                input.MaxWidth 
                    = parent.ActualWidth;
            }
        }

        //...

        protected override Size ArrangeOverride(Size finalSize)
        {
            //desiredWidth and desiredHeight are the width and height of the element that’s being adorned; these will be used to place the TransformAdorner at the corners of the adorned element.  
            var desiredWidth 
                = AdornedElement.DesiredSize.Width;
            var desiredHeight 
                = AdornedElement.DesiredSize.Height;

            //adornerWidth & adornerHeight are used for placement as well.
            var adornerWidth 
                = DesiredSize.Width;
            var adornerHeight 
                = DesiredSize.Height;

            //rotateLine.Arrange(new Rect((desiredWidth / 2) - (adornerWidth / 2), (-adornerHeight / 2) - 10, adornerWidth, adornerHeight));
            //rotateSphere.Arrange(new Rect((desiredWidth / 2) - (adornerWidth / 2), (-adornerHeight / 2) - 20, adornerWidth, adornerHeight));

            top
                .Arrange(new Rect((desiredWidth / 2) - (adornerWidth / 2), -adornerHeight / 2, adornerWidth, adornerHeight));
            left
                .Arrange(new Rect(-adornerWidth / 2, (desiredHeight / 2) - (adornerHeight / 2), adornerWidth, adornerHeight));
            right
                .Arrange(new Rect(desiredWidth - adornerWidth / 2, (desiredHeight / 2) - (adornerHeight / 2), adornerWidth, adornerHeight));
            bottom
                .Arrange(new Rect((desiredWidth / 2) - (adornerWidth / 2), desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));
            topLeft
                .Arrange(new Rect(-adornerWidth / 2, -adornerHeight / 2, adornerWidth, adornerHeight));
            topRight
                .Arrange(new Rect(desiredWidth - adornerWidth / 2, -adornerHeight / 2, adornerWidth, adornerHeight));
            bottomLeft
                .Arrange(new Rect(-adornerWidth / 2, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));
            bottomRight
                .Arrange(new Rect(desiredWidth - adornerWidth / 2, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));
            
            return finalSize;
        }

        protected override Visual GetVisualChild(int index) => Children[index];

        //...

        public void Subscribe()
        {
            top.DragDelta
                += new DragDeltaEventHandler(HandleTop);
            left.DragDelta
                += new DragDeltaEventHandler(HandleLeft);
            right.DragDelta
                += new DragDeltaEventHandler(HandleRight);
            bottom.DragDelta
                += new DragDeltaEventHandler(HandleBottom);
            topLeft.DragDelta
                += new DragDeltaEventHandler(HandleTopLeft);
            topRight.DragDelta
                += new DragDeltaEventHandler(HandleTopRight);
            bottomLeft.DragDelta
                += new DragDeltaEventHandler(HandleBottomLeft);
            bottomRight.DragDelta
                += new DragDeltaEventHandler(HandleBottomRight);
        }

        public void Unsubscribe()
        {
            top.DragDelta
                -= new DragDeltaEventHandler(HandleTop);
            left.DragDelta
                -= new DragDeltaEventHandler(HandleLeft);
            right.DragDelta
                -= new DragDeltaEventHandler(HandleRight);
            bottom.DragDelta
                -= new DragDeltaEventHandler(HandleBottom);
            topLeft.DragDelta
                -= new DragDeltaEventHandler(HandleTopLeft);
            topRight.DragDelta
                -= new DragDeltaEventHandler(HandleTopRight);
            bottomLeft.DragDelta
                -= new DragDeltaEventHandler(HandleBottomLeft);
            bottomRight.DragDelta
                -= new DragDeltaEventHandler(HandleBottomRight);
        }

        #endregion
    }
}
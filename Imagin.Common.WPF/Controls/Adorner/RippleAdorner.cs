using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Imagin.Common.Controls
{
    public enum RippleLocations
    {
        Center,
        Cursor
    }

    public class RippleAdorner : Adorner
    {
        readonly VisualCollection Children;

        readonly Ellipse Target;

        //...

        FrameworkElement Element => AdornedElement as FrameworkElement;

        protected override int VisualChildrenCount => Children.Count;

        public Point? lastPosition;

        //...

        public RippleAdorner(FrameworkElement element) : base(element)
        {
            Target = new()
            {
                Height = 0,
                Opacity = 0,
                Width = 0
            };
            Target.Bind(Ellipse.StrokeProperty, new PropertyPath("(0)", XElement.RippleStrokeProperty), element);

            Children = new(this);
            Children.Add(Target);
        }

        //...

        protected override Size ArrangeOverride(Size finalSize)
        {
            var targetWidth 
                = Target.ActualWidth / 2.0; 
            var targetHeight 
                = Target.ActualHeight / 2.0;

            var center 
                = new Point((Element.ActualWidth / 2.0) - targetWidth, (Element.ActualHeight / 2.0) - targetHeight);

            Point position = default;
            switch (XElement.GetRippleLocation(Element))
            {
                case RippleLocations.Center:
                    position = center;
                    break;

                case RippleLocations.Cursor:
                    position = lastPosition is not null
                        ? new Point(lastPosition.Value.X - targetWidth, lastPosition.Value.Y - targetHeight)
                        : center;
                    break;
            }

            Target?.Arrange(new Rect(position, new Size(Target.ActualWidth, Target.ActualHeight)));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index) => Children[index];

        //...

        static Storyboard GetStoryboard(FrameworkElement element)
        {
            var result = new Storyboard { Duration = XElement.GetRippleDuration(element) };

            var hAnimation = new DoubleAnimationUsingKeyFrames()
            {
                Duration = XElement.GetRippleDuration(element),
                AccelerationRatio = XElement.GetRippleAcceleration(element),
                DecelerationRatio = XElement.GetRippleDeceleration(element)
            };
            var wAnimation = new DoubleAnimationUsingKeyFrames()
            {
                Duration = XElement.GetRippleDuration(element),
                AccelerationRatio = XElement.GetRippleAcceleration(element),
                DecelerationRatio = XElement.GetRippleDeceleration(element)
            };
            var tAnimation = new DoubleAnimationUsingKeyFrames()
            {
                Duration = XElement.GetRippleDuration(element),
                AccelerationRatio = XElement.GetRippleAcceleration(element),
                DecelerationRatio = XElement.GetRippleDeceleration(element)
            };
            var oAnimation = new DoubleAnimationUsingKeyFrames()
            {
                Duration = XElement.GetRippleDuration(element),
                AccelerationRatio = XElement.GetRippleAcceleration(element),
                DecelerationRatio = XElement.GetRippleDeceleration(element)
            };

            Storyboard.SetTargetProperty(hAnimation,
                new PropertyPath(nameof(Ellipse.Height)));
            Storyboard.SetTargetProperty(wAnimation,
                new PropertyPath(nameof(Ellipse.Width)));
            Storyboard.SetTargetProperty(oAnimation,
                new PropertyPath(nameof(Ellipse.Opacity)));
            Storyboard.SetTargetProperty(tAnimation,
                new PropertyPath(nameof(Ellipse.StrokeThickness)));

            hAnimation.KeyFrames.Add(new DiscreteDoubleKeyFrame(0, KeyTime.FromTimeSpan(XElement.GetRippleDelay(element).TimeSpan)));
            hAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(XElement.GetRippleMaximumRadius(element), KeyTime.FromTimeSpan(XElement.GetRippleDuration(element).TimeSpan)));
            hAnimation.KeyFrames.Add(new DiscreteDoubleKeyFrame(0, KeyTime.FromTimeSpan(XElement.GetRippleDuration(element).TimeSpan)));

            wAnimation.KeyFrames.Add(new DiscreteDoubleKeyFrame(0, KeyTime.FromTimeSpan(XElement.GetRippleDelay(element).TimeSpan)));
            wAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(XElement.GetRippleMaximumRadius(element), KeyTime.FromTimeSpan(XElement.GetRippleDuration(element).TimeSpan)));
            wAnimation.KeyFrames.Add(new DiscreteDoubleKeyFrame(0, KeyTime.FromTimeSpan(XElement.GetRippleDuration(element).TimeSpan)));

            var offset = (XElement.GetRippleDuration(element).TimeSpan.Ticks / 4) * 3;

            tAnimation.KeyFrames.Add(new DiscreteDoubleKeyFrame(XElement.GetRippleStrokeThickness(element).Minimum, KeyTime.FromTimeSpan(XElement.GetRippleDelay(element).TimeSpan)));
            tAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(XElement.GetRippleStrokeThickness(element).Maximum, KeyTime.FromTimeSpan(XElement.GetRippleDuration(element).TimeSpan)));

            oAnimation.KeyFrames.Add(new DiscreteDoubleKeyFrame(XElement.GetRippleMaximumOpacity(element), KeyTime.FromTimeSpan(XElement.GetRippleDelay(element).TimeSpan)));
            oAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(XElement.GetRippleMaximumOpacity(element) / 2.0, KeyTime.FromTimeSpan(new TimeSpan(offset))));
            oAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(0.0, KeyTime.FromTimeSpan(XElement.GetRippleDuration(element).TimeSpan)));

            result.Children.Add(hAnimation);
            result.Children.Add(wAnimation);
            result.Children.Add(oAnimation);
            result.Children.Add(tAnimation);

            if (XElement.GetRippleMouseEvent(element) == MouseEvent.None)
                result.RepeatBehavior = RepeatBehavior.Forever;

            return result;
        }

        public void Ripple(Point? at)
        {
            lastPosition = at;
            Target?.BeginStoryboard(GetStoryboard(Element));
        }
    }
}
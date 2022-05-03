using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    /// <summary>
    /// To do: Not done!
    /// </summary>
    public class RotateAdorner : Adorner
    {
        //System.Windows.Shapes.Rectangle rotateLine;

        //Thumb rotateSphere;

        readonly VisualCollection Children;

        protected override int VisualChildrenCount => Children.Count;

        #region RotateAdorner

        public RotateAdorner(FrameworkElement element) : base(element)
        {
            Children
                = new VisualCollection(this);

            /*
            rotateLine = new System.Windows.Shapes.Rectangle()
            {
                Fill = Brushes.Black,
                Height = 20,
                Width = 1
            };
            visualChildren.Add(rotateLine);
            rotateSphere = new Thumb()
            {
                Background = Brushes.Black,
                Height = 10,
                BorderBrush = Brushes.White,
                BorderThickness = new Thickness(1),
                Style = rotateThumbStyle,
                Width = 10
            };
            rotateSphere.DragDelta += new DragDeltaEventHandler(OnRotating);
            rotateSphere.DragStarted += new DragStartedEventHandler(OnRotateStarted);
            visualChildren.Add(rotateSphere);
            */
        }

        #endregion

        protected override Size ArrangeOverride(Size finalSize)
        {
            var desiredWidth
                = AdornedElement.DesiredSize.Width;
            var desiredHeight
                = AdornedElement.DesiredSize.Height;

            var adornerWidth
                = DesiredSize.Width;
            var adornerHeight
                = DesiredSize.Height;

            //rotateLine.Arrange(new Rect((desiredWidth / 2) - (adornerWidth / 2), (-adornerHeight / 2) - 10, adornerWidth, adornerHeight));
            //rotateSphere.Arrange(new Rect((desiredWidth / 2) - (adornerWidth / 2), (-adornerHeight / 2) - 20, adornerWidth, adornerHeight));

            return finalSize;
        }

        protected override Visual GetVisualChild(int index) => Children[index];

        /*
        Canvas canvas;

        Point centerPoint;

        double initialAngle;

        RotateTransform rotateTransform;

        System.Windows.Vector startVector;

        void OnRotating(object sender, DragDeltaEventArgs e)
        {
            if (CanHandle(sender as Thumb))
            {
                if (Element != null && canvas != null)
                {
                    Point currentPoint = Mouse.GetPosition(canvas);
                    System.Windows.Vector deltaVector = Point.Subtract(currentPoint, centerPoint);

                    double angle = System.Windows.Vector.AngleBetween(startVector, deltaVector);

                    RotateTransform rotateTransform = Element.RenderTransform as RotateTransform;
                    rotateTransform.Angle = initialAngle + Math.Round(angle, 0);

                    Element.RenderTransformOrigin = new Point(0.5, 0.5);
                    Element.InvalidateMeasure();

                    XControl.SetRotation(Element, rotateTransform.Angle);
                }
            }
        }

        void OnRotateStarted(object sender, DragStartedEventArgs e)
        {
            if (CanHandle(sender as Thumb))
            {
                if (Element != null)
                {
                    canvas = VisualTreeHelper.GetParent(Element) as Canvas;
                    if (canvas != null)
                    {
                        centerPoint = Element.TranslatePoint(new Point(Element.Width * Element.RenderTransformOrigin.X, Element.Height * Element.RenderTransformOrigin.Y), canvas);

                        Point startPoint = Mouse.GetPosition(canvas);
                        startVector = Point.Subtract(startPoint, centerPoint);

                        rotateTransform = Element.RenderTransform as RotateTransform;
                        if (rotateTransform == null)
                        {
                            Element.RenderTransform = new RotateTransform(0);
                            initialAngle = 0;
                        }
                        else initialAngle = this.rotateTransform.Angle;
                    }
                }
            }
        }
        */
    }
}
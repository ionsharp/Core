using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Colour.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class ColorPlane : ColorViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty AlphaProperty = DependencyProperty.Register("Alpha", typeof(byte), typeof(ColorPlane), new FrameworkPropertyMetadata((byte)255, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public byte Alpha
        {
            get
            {
                return (byte)GetValue(AlphaProperty);
            }
            set
            {
                SetValue(AlphaProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectionLengthProperty = DependencyProperty.Register("SelectionLength", typeof(double), typeof(ColorPlane), new PropertyMetadata(12.0));
        /// <summary>
        /// 
        /// </summary>
        public double SelectionLength
        {
            get => (double)GetValue(SelectionLengthProperty);
            set => SetValue(SelectionLengthProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectionTypeProperty = DependencyProperty.Register("SelectionType", typeof(ColorSelectionType), typeof(ColorPlane), new PropertyMetadata(ColorSelectionType.BlackAndWhite));
        /// <summary>
        /// 
        /// </summary>
        public ColorSelectionType SelectionType
        {
            get => (ColorSelectionType)GetValue(SelectionTypeProperty);
            set => SetValue(SelectionTypeProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public ColorPlane()
        {
            InitializeComponent();
            Bitmap = new WriteableBitmap(256, 256, 96, 96, PixelFormats.Bgr24, null);
            PART_Image.Source = Bitmap;
        }

        void PosMarker(ColorSelection element, Point point)
        {
            double w = PART_Image.ActualWidth / 2, h = PART_Image.ActualHeight / 2;

            element.Selection.X = (point.X - w).Coerce(w, -w);
            element.Selection.Y = (point.Y - h).Coerce(h, -h);
        }

        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                var element = (IInputElement)sender;
                var point = e.GetPosition(element);

                PosMarker(PART_SelectionA, point);
                PosMarker(PART_SelectionB, point);

                if (Component != null)
                {
                    point = point.Coerce(new Point(255, 255));
                    SetCurrentValue(ColorProperty, Component.ColorFromPoint(point));
                }

                element.CaptureMouse();
            }
        }

        void OnMouseMove(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition((IInputElement)sender);

            PosMarker(PART_SelectionB, point);
            PART_SelectionB.Adjust();
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                PosMarker(PART_SelectionA, point);
                if (Component != null)
                {
                    point = point.Coerce(new Point(255, 255));
                    SetCurrentValue(ColorProperty, Component.ColorFromPoint(point));
                }
            }
        }

        void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Released)
                ((IInputElement)sender).ReleaseMouseCapture();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            PART_SelectionB.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            PART_SelectionB.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Draw() => Component.DrawXY(Bitmap, Component.Value.ToInt32());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        protected override void OnColorChanged(Color OldValue, Color NewValue)
        {
            if (OldValue != NewValue)
            {
                if (Component != null)
                    base.OnColorChanged(OldValue, NewValue);
            }
        }

        /*
        SelectionPoint = Value.PointFromColor(Color);
        Selection.X = SelectionPoint.X - (PART_PlaneBitmap.ActualWidth / 2);
        Selection.Y = SelectionPoint.Y - (PART_PlaneBitmap.ActualHeight / 2);
        */
    }
}
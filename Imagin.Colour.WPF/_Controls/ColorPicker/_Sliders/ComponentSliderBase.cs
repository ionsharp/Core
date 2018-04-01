using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Colour.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class ComponentSliderBase : ColorViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected Image PART_Image;

        /// <summary>
        /// 
        /// </summary>
        protected Slider PART_Slider;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty MaximumProperty = DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(ComponentSliderBase), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            protected set => SetValue(MaximumProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty MinimumProperty = DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(ComponentSliderBase), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            protected set => SetValue(MinimumProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(ComponentSliderBase), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));
        /// <summary>
        /// 
        /// </summary>
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        static void OnValueChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
            => element.To<ComponentSliderBase>().OnValueChanged((double)e.NewValue);

        /// <summary>
        /// 
        /// </summary>
        public ComponentSliderBase() : base() => DefaultStyleKey = typeof(ComponentSliderBase);

        double GetPercentage(double y) => (Maximum * (PART_Image.ActualHeight - y) / PART_Image.ActualHeight).Round();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                var element = (IInputElement)sender;
                var point = e.GetPosition(element);

                var y = point.Y.Coerce(256);
                SetCurrentValue(ValueProperty, GetPercentage(y));
                ((IInputElement)sender).CaptureMouse();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                var element = (IInputElement)sender;
                var point = e.GetPosition(element);

                var y = point.Y.Coerce(256);
                SetCurrentValue(ValueProperty, GetPercentage(y));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Released)
                ((IInputElement)sender).ReleaseMouseCapture();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Image = Template.FindName("PART_Image", this) as Image;
            PART_Image.MouseDown += OnMouseDown;
            PART_Image.MouseMove += OnMouseMove;
            PART_Image.MouseUp += OnMouseUp;
            PART_Image.Source = Bitmap;

            PART_Slider = Template.FindName("PART_Slider", this) as Slider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnValueChanged(double value)
        {
        }
    }
}

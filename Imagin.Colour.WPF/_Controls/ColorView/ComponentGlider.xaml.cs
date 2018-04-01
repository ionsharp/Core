using Imagin.Colour.Controls.Models;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Colour.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ComponentGlider : ComponentGliderBase
    {
        bool ValueChangeHandled = false;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ComponentProperty = DependencyProperty.Register("Component", typeof(Component), typeof(ComponentGlider), new FrameworkPropertyMetadata(default(Component), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnComponentChanged));
        /// <summary>
        /// 
        /// </summary>
        public Component Component
        {
            get
            {
                return (Component)GetValue(ComponentProperty);
            }
            set
            {
                SetValue(ComponentProperty, value);
            }
        }
        static void OnComponentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<ComponentGlider>().OnComponentChanged((Component)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty BitmapProperty = DependencyProperty.Register("Bitmap", typeof(WriteableBitmap), typeof(ComponentGlider), new FrameworkPropertyMetadata(default(WriteableBitmap)));
        /// <summary>
        /// 
        /// </summary>
        public WriteableBitmap Bitmap
        {
            get
            {
                return (WriteableBitmap)GetValue(BitmapProperty);
            }
            private set
            {
                SetValue(BitmapProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(ComponentGlider), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));
        /// <summary>
        /// 
        /// </summary>
        public double Value
        {
            get
            {
                return (double)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }
        static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<ComponentGlider>().OnValueChanged((double)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public ComponentGlider() : base()
        {
            SetBitmap();
            InitializeComponent();
        }

        void SetBitmap()
        {
            switch (Orientation)
            {
                case Orientation.Horizontal:
                    SetCurrentValue(BitmapProperty, new WriteableBitmap(256, 32, 96, 96, PixelFormats.Bgr24, null));
                    break;
                case Orientation.Vertical:
                    SetCurrentValue(BitmapProperty, new WriteableBitmap(32, 256, 96, 96, PixelFormats.Bgr24, null));
                    break;
            }
        }

        void UpdateValue(Point position)
        {
            var result = 0.0;
            switch (Orientation)
            {
                case Orientation.Horizontal:
                    result = ((position.X / ActualWidth) * Component.Maximum).Coerce(Component.Maximum);
                    break;
                case Orientation.Vertical:
                    result = ((position.Y / ActualHeight) * Component.Maximum).Coerce(Component.Maximum);
                    break;
            }
            SetCurrentValue(ValueProperty, result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected override void OnOrientationChanged(Orientation value)
        {
            SetBitmap();
            base.OnOrientationChanged(value);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Draw() => Component.As<VisualComponent>()?.DrawZ(Bitmap, Color, Orientation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        protected override void Update(Point position)
        {
            base.Update(position);
            UpdateValue(position);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        protected override void OnColorChanged(Color OldValue, Color NewValue)
        {
            base.OnColorChanged(OldValue, NewValue);
            if (!ColorChangeHandled)
            {
                var component = Component.As<VisualComponent>();

                ValueChangeHandled = true;

                var value = component.GetValue(NewValue);
                SetCurrentValue(ValueProperty, value);

                ValueChangeHandled = false;

                double x = .0, y = .0;
                x = value / component.Maximum * ActualWidth;
                y = value / component.Maximum * ActualHeight;

                x = ActualWidth - x;
                y = ActualHeight - y;

                UpdateMarker(new Point(x, y));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnComponentChanged(Component value) => Draw();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnValueChanged(double value)
        {
            if (!ValueChangeHandled)
            {
                ColorChangeHandled = true;
                SetCurrentValue(ColorProperty, Component.As<VisualComponent>().ColorFrom(Color, Component.Maximum - value));
                ColorChangeHandled = false;
            }
        }
    }
}


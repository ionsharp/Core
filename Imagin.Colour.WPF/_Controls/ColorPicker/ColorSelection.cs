using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Imagin.Colour.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class ColorSelection : Control
    {
        Ellipse PART_Ellipse;

        Ellipse PART_EllipseInner;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorSelection), new FrameworkPropertyMetadata(Colors.Transparent, OnColorChanged));
        /// <summary>
        /// 
        /// </summary>
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
        static void OnColorChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
            => element.To<ColorSelection>().OnColorChanged((Color)e.NewValue);

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty LengthProperty = DependencyProperty.Register(nameof(Length), typeof(double), typeof(ColorSelection), new PropertyMetadata(12.0));
        /// <summary>
        /// 
        /// </summary>
        public double Length
        {
            get => (double)GetValue(LengthProperty);
            set => SetValue(LengthProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectionProperty = DependencyProperty.Register(nameof(Selection), typeof(TranslateTransform), typeof(ColorSelection), new PropertyMetadata(default(TranslateTransform)));
        /// <summary>
        /// 
        /// </summary>
        public TranslateTransform Selection
        {
            get => (TranslateTransform)GetValue(SelectionProperty);
            private set => SetValue(SelectionProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(ColorSelectionType), typeof(ColorSelection), new PropertyMetadata(ColorSelectionType.BlackAndWhite, OnTypeChanged));
        /// <summary>
        /// 
        /// </summary>
        public ColorSelectionType Type
        {
            get => (ColorSelectionType)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }
        static void OnTypeChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
            => element.To<ColorSelection>().OnTypeChanged((ColorSelectionType)e.NewValue);

        /// <summary>
        /// 
        /// </summary>
        public ColorSelection()
        {
            DefaultStyleKey = typeof(ColorSelection);
            SetCurrentValue(SelectionProperty, new TranslateTransform());
        }

        void Adjust(Color Color)
        {
            //PART_Ellipse.Stroke = new SolidColorBrush(new HSB(Color).B > 60.0 ? Colors.Black : Colors.White);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnColorChanged(Color value)
        {
            //PosMarker(Component.PointFromColor(NewValue));
            if (Type == ColorSelectionType.BlackOrWhite)
                Adjust(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnTypeChanged(ColorSelectionType value)
        {
            if (PART_Ellipse != null && PART_EllipseInner != null)
            {
                switch (value)
                {
                    case ColorSelectionType.Black:
                        PART_Ellipse.Stroke = new SolidColorBrush(Colors.Black);
                        PART_EllipseInner.Visibility = Visibility.Collapsed;
                        break;
                    case ColorSelectionType.White:
                        PART_Ellipse.Stroke = new SolidColorBrush(Colors.White);
                        PART_EllipseInner.Visibility = Visibility.Collapsed;
                        break;
                    case ColorSelectionType.BlackAndWhite:
                        PART_Ellipse.Stroke = new SolidColorBrush(Colors.White);
                        PART_EllipseInner.Visibility = Visibility.Visible;
                        break;
                    case ColorSelectionType.BlackOrWhite:
                        Adjust();
                        PART_EllipseInner.Visibility = Visibility.Collapsed;
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Adjust() => Adjust(Color);

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_Ellipse = Template.FindName("PART_Ellipse", this) as Ellipse;
            PART_EllipseInner = Template.FindName("PART_EllipseInner", this) as Ellipse;
        }
    }
}
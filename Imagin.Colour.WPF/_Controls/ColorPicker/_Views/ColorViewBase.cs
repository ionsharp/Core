using Imagin.Colour.Controls.Models;
using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Colour.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class ColorViewBase : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        protected WriteableBitmap Bitmap;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CheckerBackgroundProperty = DependencyProperty.Register(nameof(CheckerBackground), typeof(SolidColorBrush), typeof(ColorViewBase), new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public SolidColorBrush CheckerBackground
        {
            get => (SolidColorBrush)GetValue(CheckerBackgroundProperty);
            set => SetValue(CheckerBackgroundProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CheckerForegroundProperty = DependencyProperty.Register(nameof(CheckerForeground), typeof(SolidColorBrush), typeof(ColorViewBase), new FrameworkPropertyMetadata(Brushes.LightGray, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public SolidColorBrush CheckerForeground
        {
            get => (SolidColorBrush)GetValue(CheckerForegroundProperty);
            set => SetValue(CheckerForegroundProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorViewBase), new FrameworkPropertyMetadata(default(Color), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnColorChanged));
        /// <summary>
        /// 
        /// </summary>
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
        static void OnColorChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
            => element.To<ColorViewBase>().OnColorChanged((Color)e.OldValue, (Color)e.NewValue);

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ComponentProperty = DependencyProperty.Register(nameof(Component), typeof(VisualComponent), typeof(ColorViewBase), new FrameworkPropertyMetadata(default(VisualComponent), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnComponentChanged));
        /// <summary>
        /// 
        /// </summary>
        public VisualComponent Component
        {
            get => (VisualComponent)GetValue(ComponentProperty);
            set => SetValue(ComponentProperty, value);
        }
        static void OnComponentChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
            => element.To<ColorViewBase>().OnComponentChanged((VisualComponent)e.OldValue, (VisualComponent)e.NewValue);

        /// <summary>
        /// 
        /// </summary>
        public ColorViewBase() : base() {}

        /// <summary>
        /// 
        /// </summary>
        protected virtual void Draw() {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        protected virtual void OnColorChanged(Color OldValue, Color NewValue) => Draw();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        protected virtual void OnComponentChanged(VisualComponent OldValue, VisualComponent NewValue) => Draw();
    }
}
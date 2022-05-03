using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Imagin.Common.Controls
{
    public partial class SplashWindow : Window
    {
        public static readonly ResourceKey<DropShadowEffect> DropShadowEffectKey = new();

        public event EventHandler<EventArgs> Shown;

        public static readonly DependencyProperty AccentProperty = DependencyProperty.Register(nameof(Accent), typeof(Color), typeof(SplashWindow), new FrameworkPropertyMetadata(default(Color)));
        public Color Accent
        {
            get => (Color)GetValue(AccentProperty);
            set => SetValue(AccentProperty, value);
        }

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(nameof(Image), typeof(ImageSource), typeof(SplashWindow), new FrameworkPropertyMetadata(null));
        public ImageSource Image
        {
            get => (ImageSource)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof(Message), typeof(string), typeof(SplashWindow), new FrameworkPropertyMetadata(null));
        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(nameof(Progress), typeof(double), typeof(SplashWindow), new FrameworkPropertyMetadata(0d));
        public double Progress
        {
            get => (double)GetValue(ProgressProperty);
            set => SetValue(ProgressProperty, value);
        }

        static SplashWindow()
        {
            OpacityProperty
                .OverrideMetadata(typeof(SplashWindow), new FrameworkPropertyMetadata(0d, OnOpacityChanged));
            WindowStateProperty
                .OverrideMetadata(typeof(SplashWindow), new FrameworkPropertyMetadata(WindowState.Normal, null, OnWindowStateCoerced));
        }

        public SplashWindow() : base() => InitializeComponent();

        static void OnOpacityChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if ((double)e.NewValue == 1)
            {
                if (sender is SplashWindow window)
                    window.Shown?.Invoke(window, EventArgs.Empty);
            }
        }

        static object OnWindowStateCoerced(DependencyObject sender, object input) => WindowState.Normal;
    }
}
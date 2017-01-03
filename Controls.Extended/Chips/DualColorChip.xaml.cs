using Imagin.Common.Input;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public partial class DualColorChip : UserControl
    {
        public DualColorChip()
        {
            InitializeComponent();
        }

        public event EventHandler<EventArgs<Color>> ForegroundColorChanged;

        public event EventHandler<EventArgs<Color>> BackgroundColorChanged;

        public static DependencyProperty DefaultForegroundProperty = DependencyProperty.Register("DefaultForeground", typeof(Color), typeof(DualColorChip), new PropertyMetadata(Colors.Black));
        public Color DefaultForeground
        {
            get
            {
                return (Color)GetValue(DefaultForegroundProperty);
            }
            set
            {
                SetValue(DefaultForegroundProperty, value);
            }
        }

        public static DependencyProperty DefaultBackgroundProperty = DependencyProperty.Register("DefaultBackground", typeof(Color), typeof(DualColorChip), new PropertyMetadata(Colors.White));
        public Color DefaultBackground
        {
            get
            {
                return (Color)GetValue(DefaultBackgroundProperty);
            }
            set
            {
                SetValue(DefaultBackgroundProperty, value);
            }
        }

        public static DependencyProperty ForegroundColorProperty = DependencyProperty.Register("ForegroundColor", typeof(Color), typeof(DualColorChip), new PropertyMetadata(Colors.Black, OnForegroundColorChanged));
        public Color ForegroundColor
        {
            get
            {
                return (Color)GetValue(ForegroundColorProperty);
            }
            set
            {
                SetValue(ForegroundColorProperty, value);
            }
        }
        static void OnForegroundColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var DualColorChip = (DualColorChip)d;
            if (DualColorChip.ForegroundColorChanged != null)
                DualColorChip.ForegroundColorChanged(DualColorChip, new EventArgs<Color>((Color)e.NewValue));
        }

        public static DependencyProperty BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(Color), typeof(DualColorChip), new PropertyMetadata(Colors.White, OnBackgroundColorChanged));
        public Color BackgroundColor
        {
            get
            {
                return (Color)GetValue(BackgroundColorProperty);
            }
            set
            {
                SetValue(BackgroundColorProperty, value);
            }
        }
        static void OnBackgroundColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var DualColorChip = (DualColorChip)d;
            if (DualColorChip.BackgroundColorChanged != null)
                DualColorChip.BackgroundColorChanged(DualColorChip, new EventArgs<Color>((Color)e.NewValue));
        }

        void OnDefaultMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.ForegroundColor = Color.FromArgb(this.DefaultForeground.A, this.DefaultForeground.R, this.DefaultForeground.G, this.DefaultForeground.B);
            this.BackgroundColor = Color.FromArgb(this.DefaultBackground.A, this.DefaultBackground.R, this.DefaultBackground.G, this.DefaultBackground.B);
        }

        void OnSwitchMouseDown(object sender, MouseButtonEventArgs e)
        {
            var Foreground = Color.FromArgb(this.ForegroundColor.A, this.ForegroundColor.R, this.ForegroundColor.G, this.ForegroundColor.B);
            this.ForegroundColor = Color.FromArgb(this.BackgroundColor.A, this.BackgroundColor.R, this.BackgroundColor.G, this.BackgroundColor.B);
            this.BackgroundColor = Foreground;
        }
    }
}

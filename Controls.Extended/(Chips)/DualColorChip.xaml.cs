using Imagin.Common.Input;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Imagin.Common.Extensions;
using Imagin.Common.Globalization;

namespace Imagin.Controls.Extended
{
    public partial class DualColorChip : UserControl
    {
        public event EventHandler<EventArgs<Color>> ForegroundColorChanged;

        public event EventHandler<EventArgs<Color>> BackgroundColorChanged;

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
            d.As<DualColorChip>().OnBackgroundColorChanged((Color)e.NewValue);
        }

        public static DependencyProperty BackgroundToolTipProperty = DependencyProperty.Register("BackgroundToolTip", typeof(string), typeof(DualColorChip), new PropertyMetadata("Background"));
        public string BackgroundToolTip
        {
            get
            {
                return (string)GetValue(BackgroundToolTipProperty);
            }
            set
            {
                SetValue(BackgroundToolTipProperty, value);
            }
        }

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
            d.As<DualColorChip>().OnForegroundColorChanged((Color)e.NewValue);
        }

        public static DependencyProperty ForegroundToolTipProperty = DependencyProperty.Register("ForegroundToolTip", typeof(string), typeof(DualColorChip), new PropertyMetadata("Foreground"));
        public string ForegroundToolTip
        {
            get
            {
                return (string)GetValue(ForegroundToolTipProperty);
            }
            set
            {
                SetValue(ForegroundToolTipProperty, value);
            }
        }

        public static DependencyProperty ResetToolTipProperty = DependencyProperty.Register("ResetToolTip", typeof(string), typeof(DualColorChip), new PropertyMetadata("Reset"));
        public string ResetToolTip
        {
            get
            {
                return (string)GetValue(ResetToolTipProperty);
            }
            set
            {
                SetValue(ResetToolTipProperty, value);
            }
        }

        public static DependencyProperty SwitchToolTipProperty = DependencyProperty.Register("SwitchToolTip", typeof(string), typeof(DualColorChip), new PropertyMetadata("Swap"));
        public string SwitchToolTip
        {
            get
            {
                return (string)GetValue(SwitchToolTipProperty);
            }
            set
            {
                SetValue(SwitchToolTipProperty, value);
            }
        }
        
        public DualColorChip()
        {
            InitializeComponent();
        }

        void OnDefaultMouseDown(object sender, MouseButtonEventArgs e)
        {
            ForegroundColor = DefaultForeground;
            BackgroundColor = DefaultBackground;
        }

        void OnSwitchMouseDown(object sender, MouseButtonEventArgs e)
        {
            var a = ForegroundColor;
            var b = BackgroundColor;

            ForegroundColor = b;
            BackgroundColor = a;
        }

        protected virtual void OnBackgroundColorChanged(Color Value)
        {
            if (BackgroundColorChanged != null)
                BackgroundColorChanged(this, new EventArgs<Color>(Value));
        }

        protected virtual void OnForegroundColorChanged(Color Value)
        {
            if (ForegroundColorChanged != null)
                ForegroundColorChanged(this, new EventArgs<Color>((Color)Value));
        }
    }
}

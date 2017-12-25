using Imagin.Common.Input;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Imagin.Common.Linq;
using Imagin.Common.Globalization;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DualColorChip : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs<Color>> ForegroundColorChanged;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs<Color>> BackgroundColorChanged;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(Color), typeof(DualColorChip), new PropertyMetadata(Colors.White, OnBackgroundColorChanged));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty BackgroundToolTipProperty = DependencyProperty.Register("BackgroundToolTip", typeof(string), typeof(DualColorChip), new PropertyMetadata("Background"));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DefaultBackgroundProperty = DependencyProperty.Register("DefaultBackground", typeof(Color), typeof(DualColorChip), new PropertyMetadata(Colors.White));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DefaultForegroundProperty = DependencyProperty.Register("DefaultForeground", typeof(Color), typeof(DualColorChip), new PropertyMetadata(Colors.Black));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ForegroundColorProperty = DependencyProperty.Register("ForegroundColor", typeof(Color), typeof(DualColorChip), new PropertyMetadata(Colors.Black, OnForegroundColorChanged));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ForegroundToolTipProperty = DependencyProperty.Register("ForegroundToolTip", typeof(string), typeof(DualColorChip), new PropertyMetadata("Foreground"));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ResetToolTipProperty = DependencyProperty.Register("ResetToolTip", typeof(string), typeof(DualColorChip), new PropertyMetadata("Reset"));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SwitchToolTipProperty = DependencyProperty.Register("SwitchToolTip", typeof(string), typeof(DualColorChip), new PropertyMetadata("Swap"));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public DualColorChip()
        {
            InitializeComponent();
        }

        void OnDefaultMouseDown(object sender, MouseButtonEventArgs e)
        {
            BackgroundColor = DefaultBackground;
            ForegroundColor = DefaultForeground;
        }

        void OnSwitchMouseDown(object sender, MouseButtonEventArgs e)
        {
            var a = ForegroundColor;
            var b = BackgroundColor;

            ForegroundColor = b;
            BackgroundColor = a;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnBackgroundColorChanged(Color Value)
        {
            BackgroundColorChanged?.Invoke(this, new EventArgs<Color>(Value));
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnForegroundColorChanged(Color Value)
        {
            ForegroundColorChanged?.Invoke(this, new EventArgs<Color>(Value));
        }
    }
}

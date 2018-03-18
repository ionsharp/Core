using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Imagin.Colour.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class DualColorChip : Control
    {
        Grid PART_Reset;

        Rectangle PART_Switch;

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
        public static DependencyProperty BackgroundColorProperty = DependencyProperty.Register(nameof(BackgroundColor), typeof(Color), typeof(DualColorChip), new PropertyMetadata(Colors.White, OnBackgroundColorChanged));
        /// <summary>
        /// 
        /// </summary>
        public Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }
        static void OnBackgroundColorChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            element.As<DualColorChip>().OnBackgroundColorChanged((Color)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty BackgroundToolTipProperty = DependencyProperty.Register(nameof(BackgroundToolTip), typeof(string), typeof(DualColorChip), new PropertyMetadata("Background"));
        /// <summary>
        /// 
        /// </summary>
        public string BackgroundToolTip
        {
            get => (string)GetValue(BackgroundToolTipProperty);
            set => SetValue(BackgroundToolTipProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DefaultBackgroundProperty = DependencyProperty.Register(nameof(DefaultBackground), typeof(Color), typeof(DualColorChip), new PropertyMetadata(Colors.White));
        /// <summary>
        /// 
        /// </summary>
        public Color DefaultBackground
        {
            get => (Color)GetValue(DefaultBackgroundProperty);
            set => SetValue(DefaultBackgroundProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DefaultForegroundProperty = DependencyProperty.Register(nameof(DefaultForeground), typeof(Color), typeof(DualColorChip), new PropertyMetadata(Colors.Black));
        /// <summary>
        /// 
        /// </summary>
        public Color DefaultForeground
        {
            get => (Color)GetValue(DefaultForegroundProperty);
            set => SetValue(DefaultForegroundProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ForegroundColorProperty = DependencyProperty.Register(nameof(ForegroundColor), typeof(Color), typeof(DualColorChip), new PropertyMetadata(Colors.Black, OnForegroundColorChanged));
        /// <summary>
        /// 
        /// </summary>
        public Color ForegroundColor
        {
            get => (Color)GetValue(ForegroundColorProperty);
            set => SetValue(ForegroundColorProperty, value);
        }
        static void OnForegroundColorChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            element.As<DualColorChip>().OnForegroundColorChanged((Color)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ForegroundToolTipProperty = DependencyProperty.Register(nameof(ForegroundToolTip), typeof(string), typeof(DualColorChip), new PropertyMetadata("Foreground"));
        /// <summary>
        /// 
        /// </summary>
        public string ForegroundToolTip
        {
            get => (string)GetValue(ForegroundToolTipProperty);
            set => SetValue(ForegroundToolTipProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ResetToolTipProperty = DependencyProperty.Register(nameof(ResetToolTip), typeof(string), typeof(DualColorChip), new PropertyMetadata("Reset"));
        /// <summary>
        /// 
        /// </summary>
        public string ResetToolTip
        {
            get => (string)GetValue(ResetToolTipProperty);
            set => SetValue(ResetToolTipProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SwitchToolTipProperty = DependencyProperty.Register(nameof(SwitchToolTip), typeof(string), typeof(DualColorChip), new PropertyMetadata("Swap"));
        /// <summary>
        /// 
        /// </summary>
        public string SwitchToolTip
        {
            get => (string)GetValue(SwitchToolTipProperty);
            set => SetValue(SwitchToolTipProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public DualColorChip() => DefaultStyleKey = typeof(DualColorChip);

        void OnResetMouseDown(object sender, MouseButtonEventArgs e)
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
        protected virtual void OnBackgroundColorChanged(Color value)
            => BackgroundColorChanged?.Invoke(this, new EventArgs<Color>(value));

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnForegroundColorChanged(Color value)
            => ForegroundColorChanged?.Invoke(this, new EventArgs<Color>(value));

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Reset = Template.FindName("PART_Reset", this) as Grid;
            PART_Reset.PreviewMouseDown += OnResetMouseDown;

            PART_Switch = Template.FindName("PART_Switch", this) as Rectangle;
            PART_Switch.PreviewMouseDown += OnSwitchMouseDown;
        }
    }
}
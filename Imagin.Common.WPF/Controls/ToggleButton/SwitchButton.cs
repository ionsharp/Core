using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class SwitchButton : CheckBox
    {
        public static readonly DependencyProperty SymbolForegroundProperty = DependencyProperty.Register(nameof(SymbolForeground), typeof(Brush), typeof(SwitchButton), new FrameworkPropertyMetadata(Brushes.Black));
        public Brush SymbolForeground
        {
            get => (Brush)GetValue(SymbolForegroundProperty);
            set => SetValue(SymbolForegroundProperty, value);
        }

        public static readonly DependencyProperty SymbolSizeProperty = DependencyProperty.Register(nameof(SymbolSize), typeof(double), typeof(SwitchButton), new FrameworkPropertyMetadata(10.0));
        public double SymbolSize
        {
            get => (double)GetValue(SymbolSizeProperty);
            set => SetValue(SymbolSizeProperty, value);
        }

        public static readonly DependencyProperty ThumbStyleProperty = DependencyProperty.Register(nameof(ThumbStyle), typeof(Style), typeof(SwitchButton), new FrameworkPropertyMetadata(default(Style)));
        public Style ThumbStyle
        {
            get => (Style)GetValue(ThumbStyleProperty);
            set => SetValue(ThumbStyleProperty, value);
        }

        public SwitchButton() : base() { }
    }
}
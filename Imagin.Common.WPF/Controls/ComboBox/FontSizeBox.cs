using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class FontSizeBox : ComboBox
    {
        public static double[] DefaultSizes => new double[]
        {
            8d, 9d, 10d, 12d, 14d, 16d, 18d, 20d, 22d, 24d, 26d, 28d, 36d, 48d, 72d
        };

        public static readonly DependencyProperty SizeIncrementProperty = DependencyProperty.Register(nameof(SizeIncrement), typeof(double), typeof(FontSizeBox), new FrameworkPropertyMetadata(0d));
        public double SizeIncrement
        {
            get => (double)GetValue(SizeIncrementProperty);
            set => SetValue(SizeIncrementProperty, value);
        }

        public static readonly DependencyProperty SizeMaximumProperty = DependencyProperty.Register(nameof(SizeMaximum), typeof(double), typeof(FontSizeBox), new FrameworkPropertyMetadata(72d));
        public double SizeMaximum
        {
            get => (double)GetValue(SizeMaximumProperty);
            set => SetValue(SizeMaximumProperty, value);
        }

        public static readonly DependencyProperty SizeMinimumProperty = DependencyProperty.Register(nameof(SizeMinimum), typeof(double), typeof(FontSizeBox), new FrameworkPropertyMetadata(1d));
        public double SizeMinimum
        {
            get => (double)GetValue(SizeMinimumProperty);
            set => SetValue(SizeMinimumProperty, value);
        }

        public static readonly DependencyProperty SizesProperty = DependencyProperty.Register(nameof(Sizes), typeof(DoubleCollection), typeof(FontSizeBox), new FrameworkPropertyMetadata(null));
        public DoubleCollection Sizes
        {
            get => (DoubleCollection)GetValue(SizesProperty);
            set => SetValue(SizesProperty, value);
        }

        public FontSizeBox() : base() => SetCurrentValue(SizesProperty, new DoubleCollection(DefaultSizes));
    }
}
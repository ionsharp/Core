using Imagin.Common.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// A ComboBox containing traditional font sizes.
    /// </summary>
    public partial class FontSizeBox : ComboBox
    {
        public static double[] Default
        {
            get
            {
                return new double[]
                {
                    8d, 9d, 10d, 12d, 14d, 16d, 18d, 20d, 22d, 24d, 26d, 28d, 36d, 48d, 72d
                };
            }
        }

        public static DependencyProperty WheelTickMaxProperty = DependencyProperty.Register("WheelTickMax", typeof(double), typeof(FontSizeBox), new FrameworkPropertyMetadata(72d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double WheelTickMax
        {
            get
            {
                return (double)GetValue(WheelTickMaxProperty);
            }
            set
            {
                SetValue(WheelTickMaxProperty, value);
            }
        }

        public static DependencyProperty WheelTickMinProperty = DependencyProperty.Register("WheelTickMin", typeof(double), typeof(FontSizeBox), new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double WheelTickMin
        {
            get
            {
                return (double)GetValue(WheelTickMinProperty);
            }
            set
            {
                SetValue(WheelTickMinProperty, value);
            }
        }

        public static DependencyProperty SizesProperty = DependencyProperty.Register("Sizes", typeof(DoubleCollection), typeof(FontSizeBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DoubleCollection Sizes
        {
            get
            {
                return (DoubleCollection)GetValue(SizesProperty);
            }
            set
            {
                SetValue(SizesProperty, value);
            }
        }

        public static DependencyProperty WheelTickProperty = DependencyProperty.Register("WheelTick", typeof(double), typeof(FontSizeBox), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Value to incremenet/decrement on mouse wheel; set 0 to stick to default values.
        /// </summary>
        public double WheelTick
        {
            get
            {
                return (double)GetValue(WheelTickProperty);
            }
            set
            {
                SetValue(WheelTickProperty, value);
            }
        }

        public FontSizeBox() : this(Default)
        {
        }

        public FontSizeBox(double[] Values) : base()
        {
            InitializeComponent();
            Sizes = new DoubleCollection();
            Set(Values);
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) > 0)
            {
                var Value = SelectedValue.To<double>();

                if (e.Delta > 0)
                {
                    if (WheelTick == 0)
                    {
                        var i = Sizes.IndexOf(SelectedValue.To<double>()) + 1;
                        if (i <= Sizes.Count - 1)
                            Value = Sizes[i];
                    }
                    else
                    {
                        if (Value + WheelTick <= WheelTickMax)
                            Value += WheelTick;
                    }
                }
                else
                {
                    if (WheelTick == 0)
                    {
                        var i = Sizes.IndexOf(SelectedValue.To<double>()) - 1;
                        if (i >= 0)
                            Value = Sizes[i];
                    }
                    else
                    {
                        if (Value - WheelTick >= WheelTickMin)
                            Value -= WheelTick;
                    }
                }

                SelectedValue = Value;
            }
        }

        public void Set(double[] Values)
        {
            Sizes.Clear();
            foreach (var i in Values)
                Sizes.Add(i);
        }
    }
}

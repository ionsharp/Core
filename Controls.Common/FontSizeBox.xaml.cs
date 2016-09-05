using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// A ComboBox containing font sizes.
    /// </summary>
    public partial class FontSizeBox : ComboBox
    {
        public double[] Default
        {
            get
            {
                return new double[]
                {
                    8.0,
                    9.0,
                    10.0,
                    12.0,
                    14.0,
                    16.0,
                    18.0,
                    20.0,
                    22.0,
                    24.0,
                    26.0,
                    28.0,
                    36.0,
                    48.0,
                    72.0
                };
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

        public FontSizeBox()
        {
            InitializeComponent();

            this.Sizes = new DoubleCollection();
            this.Reset();
        }

        public void Reset()
        {
            this.Sizes.Clear();
            foreach (double d in this.Default)
                this.Sizes.Add(d);
        }
    }
}

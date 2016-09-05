using System.Text.RegularExpressions;
using System.Windows;

namespace Imagin.Controls.Common
{
    public abstract class FloatingPointUpDown : NumericUpDown
    {
        public override Regex Expression
        {
            get
            {
                return new Regex("^[0-9]?$");
            }
        }

        public static DependencyProperty PrecisionProperty = DependencyProperty.Register("Precision", typeof(int), typeof(FloatingPointUpDown), new FrameworkPropertyMetadata(3, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public int Precision
        {
            get
            {
                return (int)GetValue(PrecisionProperty);
            }
            set
            {
                SetValue(PrecisionProperty, value);
            }
        }

        public FloatingPointUpDown() : base()
        {
        }
    }
}

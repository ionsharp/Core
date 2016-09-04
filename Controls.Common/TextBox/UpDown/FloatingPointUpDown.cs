using Imagin.Common.Extensions;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        public static DependencyProperty MantissaProperty = DependencyProperty.Register("Mantissa", typeof(int), typeof(FloatingPointUpDown), new FrameworkPropertyMetadata(3, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnMantissaChanged));
        public int Mantissa
        {
            get
            {
                return (int)GetValue(MantissaProperty);
            }
            set
            {
                SetValue(MantissaProperty, value);
            }
        }
        static void OnMantissaChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            FloatingPointUpDown FloatingPointUpDown = (FloatingPointUpDown)Object;
            FloatingPointUpDown.FormatValue(FloatingPointUpDown.StringFormat);
        }

        public FloatingPointUpDown() : base()
        {
            this.StringFormat = "N3";
        }
    }
}

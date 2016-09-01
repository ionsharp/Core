using Imagin.Common.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    public class DoubleTextBox : TextBox
    {
        public double Value
        {
            get
            {
                return this.Text.ToDouble();
            }
        }

        public static DependencyProperty MantissaProperty = DependencyProperty.Register("Mantissa", typeof(int), typeof(DoubleTextBox), new FrameworkPropertyMetadata(3, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public DoubleTextBox() : base()
        {
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            double Value;
            int CaretIndex = this.CaretIndex;
            if (string.IsNullOrWhiteSpace(this.Text))
                this.Text = 0.0.ToString();
            else if (double.TryParse(this.Text, out Value))
                this.Text = Value.ToString("N" + Mantissa.ToString());
            this.CaretIndex = CaretIndex;
        }
    }
}

using Imagin.Common.Extensions;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        public static DependencyProperty MantissaProperty = DependencyProperty.Register("Mantissa", typeof(int), typeof(DoubleTextBox), new FrameworkPropertyMetadata(3, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnMantissaChanged));
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
            DoubleTextBox DoubleTextBox = (DoubleTextBox)Object;
            DoubleTextBox.SetText();
        }

        public DoubleTextBox() : base()
        {
        }

        void SetText()
        {
            double Value;
            if (string.IsNullOrWhiteSpace(this.Text))
                this.Text = 0.0.ToString();
            else if (double.TryParse(this.Text, out Value))
                this.Text = Value.ToString("N" + Mantissa.ToString());
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            Regex r = new Regex(@"^\d$");
            e.Handled = !r.IsMatch(e.Text);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            int CaretIndex = this.CaretIndex;
            this.SetText();
            this.CaretIndex = CaretIndex;
        }
    }
}

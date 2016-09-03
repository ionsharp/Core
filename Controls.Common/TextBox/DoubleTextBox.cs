using Imagin.Common.Extensions;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public class DoubleTextBox : AdvancedTextBox
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
            DoubleTextBox.SetValue(DoubleTextBox.Value);
        }

        public DoubleTextBox() : base()
        {
        }

        void SetValue(double NewValue)
        {
            this.Text = NewValue.ToString("N" + Mantissa.ToString());
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
            this.SetValue(this.Value);
            this.CaretIndex = CaretIndex;
        }
    }
}

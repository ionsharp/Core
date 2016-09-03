using Imagin.Common.Extensions;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public class DoubleUpDown : UpDown
    {
        #region Properties

        public double Value
        {
            get
            {
                if (string.IsNullOrEmpty(this.Text))
                    return 0.0;
                else return this.Text.ToDouble();
            }
        }

        public static DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(DoubleUpDown), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double Minimum
        {
            get
            {
                return (double)GetValue(MinimumProperty);
            }
            set
            {
                SetValue(MinimumProperty, value);
            }
        }

        public static DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(DoubleUpDown), new FrameworkPropertyMetadata(1000d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double Maximum
        {
            get
            {
                return (double)GetValue(MaximumProperty);
            }
            set
            {
                SetValue(MaximumProperty, value);
            }
        }

        public static DependencyProperty MantissaProperty = DependencyProperty.Register("Mantissa", typeof(int), typeof(DoubleUpDown), new FrameworkPropertyMetadata(3, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnMantissaChanged));
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
            DoubleUpDown DoubleUpDown = (DoubleUpDown)Object;
            DoubleUpDown.FormatValue();
        }

        #endregion

        #region DoubleUpDown

        public DoubleUpDown() : base()
        {
        }

        #endregion

        #region Methods

        void SetValue(double NewValue)
        {
            this.Text = NewValue.ToString();
        }

        void FormatValue()
        {
            int CaretIndex = this.CaretIndex;
            this.Text = this.Value.ToString("N" + Mantissa.ToString());
            this.CaretIndex = CaretIndex;
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            Regex r = new Regex("^[0-9-.]?$");
            e.Handled = !r.IsMatch(e.Text);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            this.FormatValue();
        }

        protected override void Up_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.SetValue(this.Value + 1.0);
        }

        protected override void Down_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.SetValue(this.Value - 1.0);
        }

        protected override void Up_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.Value < this.Maximum;
        }

        protected override void Down_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.Value > this.Minimum;
        }

        #endregion
    }
}

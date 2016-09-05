using Imagin.Common.Extensions;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public class DoubleUpDown : FloatingPointUpDown
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
        
        public static DependencyProperty IncrementProperty = DependencyProperty.Register("Increment", typeof(double), typeof(DoubleUpDown), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double Increment
        {
            get
            {
                return (double)GetValue(IncrementProperty);
            }
            set
            {
                SetValue(IncrementProperty, value);
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

        #endregion

        #region DoubleUpDown

        public DoubleUpDown() : base()
        {
        }

        #endregion

        #region Methods

        public override object GetValue()
        {
            return this.Value;
        }

        protected override void CoerceValue(object NewValue)
        {
            if (NewValue.As<double>() < this.Minimum)
                this.SetText(this.Minimum, true);
            if (NewValue.As<double>() > this.Maximum)
                this.SetText(this.Maximum, true);
        }

        protected override void FormatValue()
        {
            this.SetText(string.Format("{0:0." + new string('0', this.Precision) + "}", this.Value), true);
        }

        public override void Increase()
        {
            this.SetText(this.Value + this.Increment);
        }
        protected override void CanIncrease(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.Value < this.Maximum;
        }

        public override void Decrease()
        {
            this.SetText(this.Value - this.Increment);
        }
        protected override void CanDecrease(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.Value > this.Minimum;
        }

        #endregion
    }
}

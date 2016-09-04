using Imagin.Common.Extensions;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
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

        protected override void FormatValue(string StringFormat)
        {
            if (!string.IsNullOrEmpty(StringFormat))
                this.SetText(this.Value.ToString(StringFormat), true);
            else this.SetText(this.Value.ToString(string.Concat("N", this.Mantissa.ToString())), true);
        }

        protected override void Up_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.SetText(this.Value + this.Increment);
        }
        protected override void Up_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.Value < this.Maximum;
        }

        protected override void Down_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.SetText(this.Value - this.Increment);
        }
        protected override void Down_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.Value > this.Minimum;
        }

        #endregion
    }
}

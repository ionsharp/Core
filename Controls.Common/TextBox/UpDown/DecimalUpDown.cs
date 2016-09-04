using Imagin.Common.Extensions;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public class DecimalUpDown : FloatingPointUpDown
    {
        #region Properties

        public decimal Value
        {
            get
            {
                if (string.IsNullOrEmpty(this.Text))
                    return 0m;
                else return this.Text.ToDecimal();
            }
        }

        public static DependencyProperty IncrementProperty = DependencyProperty.Register("Increment", typeof(decimal), typeof(DecimalUpDown), new FrameworkPropertyMetadata(1m, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public decimal Increment
        {
            get
            {
                return (decimal)GetValue(IncrementProperty);
            }
            set
            {
                SetValue(IncrementProperty, value);
            }
        }

        public static DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(decimal), typeof(DecimalUpDown), new FrameworkPropertyMetadata(0m, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public decimal Minimum
        {
            get
            {
                return (decimal)GetValue(MinimumProperty);
            }
            set
            {
                SetValue(MinimumProperty, value);
            }
        }

        public static DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(decimal), typeof(DecimalUpDown), new FrameworkPropertyMetadata(1000m, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public decimal Maximum
        {
            get
            {
                return (decimal)GetValue(MaximumProperty);
            }
            set
            {
                SetValue(MaximumProperty, value);
            }
        }

        #endregion

        #region DecimalUpDown

        public DecimalUpDown() : base()
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
            if (NewValue.As<decimal>() < this.Minimum)
                this.SetText(this.Minimum, true);
            if (NewValue.As<decimal>() > this.Maximum)
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

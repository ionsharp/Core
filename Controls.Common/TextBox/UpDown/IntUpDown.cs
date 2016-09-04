using Imagin.Common.Extensions;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public class IntUpDown : NumericUpDown
    {
        public override Regex Expression
        {
            get
            {
                return new Regex("^[0-9]?$");
            }
        }

        public int Value
        {
            get
            {
                if (string.IsNullOrEmpty(this.Text))
                    return 0;
                else return this.Text.ToInt();
            }
        }

        public static DependencyProperty IncrementProperty = DependencyProperty.Register("Increment", typeof(int), typeof(IntUpDown), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public int Increment
        {
            get
            {
                return (int)GetValue(IncrementProperty);
            }
            set
            {
                SetValue(IncrementProperty, value);
            }
        }

        public static DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(int), typeof(IntUpDown), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public int Minimum
        {
            get
            {
                return (int)GetValue(MinimumProperty);
            }
            set
            {
                SetValue(MinimumProperty, value);
            }
        }

        public static DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(int), typeof(IntUpDown), new FrameworkPropertyMetadata(1000, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public int Maximum
        {
            get
            {
                return (int)GetValue(MaximumProperty);
            }
            set
            {
                SetValue(MaximumProperty, value);
            }
        }

        public IntUpDown() : base()
        {
        }

        void CoeerceValue(int Value)
        {
            if (Value < this.Minimum)
                this.Text = this.Minimum.ToString();
            if (Value > this.Maximum)
                this.Text = this.Maximum.ToString();
        }

        public override object GetValue()
        {
            return this.Value;
        }

        protected override void CoerceValue(object NewValue)
        {
            if (NewValue.As<int>() < this.Minimum)
                this.SetText(this.Minimum, true);
            if (NewValue.As<int>() > this.Maximum)
                this.SetText(this.Maximum, true);
        }

        protected override void FormatValue(string StringFormat)
        {
            if (!string.IsNullOrEmpty(StringFormat))
                this.SetText(this.Value.ToString(StringFormat), true);
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
    }
}

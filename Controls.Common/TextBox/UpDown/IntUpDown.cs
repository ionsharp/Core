using Imagin.Common.Extensions;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public class IntUpDown : NumericUpDown
    {
        #region Properties

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

        #endregion

        #region IntUpDown

        public IntUpDown() : base()
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
            if (NewValue.As<int>() < this.Minimum)
                this.SetText(this.Minimum, true);
            if (NewValue.As<int>() > this.Maximum)
                this.SetText(this.Maximum, true);
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

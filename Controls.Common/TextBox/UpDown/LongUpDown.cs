using Imagin.Common.Extensions;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public class LongUpDown : UpDown
    {
        public long Value
        {
            get
            {
                if (string.IsNullOrEmpty(this.Text))
                    return 0L;
                else return this.Text.ToLong();
            }
        }

        public static DependencyProperty IncrementProperty = DependencyProperty.Register("Increment", typeof(long), typeof(LongUpDown), new FrameworkPropertyMetadata(1L, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public long Increment
        {
            get
            {
                return (long)GetValue(IncrementProperty);
            }
            set
            {
                SetValue(IncrementProperty, value);
            }
        }

        public static DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(long), typeof(LongUpDown), new FrameworkPropertyMetadata(0L, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public long Minimum
        {
            get
            {
                return (long)GetValue(MinimumProperty);
            }
            set
            {
                SetValue(MinimumProperty, value);
            }
        }

        public static DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(long), typeof(LongUpDown), new FrameworkPropertyMetadata(1000000L, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public long Maximum
        {
            get
            {
                return (long)GetValue(MaximumProperty);
            }
            set
            {
                SetValue(MaximumProperty, value);
            }
        }

        public LongUpDown() : base()
        {
        }

        void CoeerceValue(long Value)
        {
            if (Value < this.Minimum)
                this.Text = this.Minimum.ToString();
            if (Value > this.Maximum)
                this.Text = this.Maximum.ToString();
        }

        protected override object GetValue()
        {
            return this.Value;
        }

        protected override void CoerceValue(object NewValue)
        {
            if (Value < this.Minimum)
                this.Text = this.Minimum.ToString();
            if (Value > this.Maximum)
                this.Text = this.Maximum.ToString();
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            Regex r = new Regex("^[0-9]?$");
            e.Handled = !r.IsMatch(e.Text);
        }

        protected override void Up_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            long NewValue = 0L;
            if ((NewValue = this.Value + this.Increment) > this.Maximum)
                this.Text = this.Maximum.ToString();
            else this.Text = NewValue.ToString();
        }
        protected override void Up_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.Value < this.Maximum;
        }

        protected override void Down_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            long NewValue = 0L;
            if ((NewValue = this.Value - this.Increment) < this.Minimum)
                this.Text = this.Minimum.ToString();
            else this.Text = NewValue.ToString();
        }
        protected override void Down_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.Value > this.Minimum;
        }
    }
}

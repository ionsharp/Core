using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Imagin.Common.Extensions;

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

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            Regex r = new Regex("^[0-9]?$");
            e.Handled = !r.IsMatch(e.Text);
        }

        protected override void Up_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.Text))
                this.Text = 0L.ToString();
            else this.Text = (this.Text.ToLong() + 1L).ToString();
        }

        protected override void Down_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.Text))
                this.Text = 0L.ToString();
            else this.Text = (this.Text.ToLong() - 1L).ToString();
        }

        protected override void Up_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.Value < this.Maximum;
        }

        protected override void Down_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.Value > this.Minimum;
        }
    }
}

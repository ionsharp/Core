using System.Windows;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public class LongUpDown : UpDown
    {
        public static DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(long), typeof(DoubleUpDown), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(long), typeof(DoubleUpDown), new FrameworkPropertyMetadata(100d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
            else this.Text = (Convert.ToInt64(this.Text) + 1L).ToString();
        }

        protected override void Down_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.Text))
                this.Text = 0L.ToString();
            else this.Text = (Convert.ToInt64(this.Text) - 1L).ToString();
        }
    }
}

using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public class DoubleUpDown : UpDown
    {
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

        public static DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(DoubleUpDown), new FrameworkPropertyMetadata(100d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public DoubleUpDown() : base()
        {
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            Regex r = new Regex("^[0-9-.]?$");
            e.Handled = !r.IsMatch(e.Text);
        }

        protected override void Up_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.Text))
                this.Text = 0.ToString();
            else
                this.Text = (Convert.ToDouble(this.Text) + 1.0).ToString();
        }

        protected override void Down_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.Text))
                this.Text = 0d.ToString();
            else
                this.Text = (Convert.ToDouble(this.Text) - 1.0).ToString();
        }
    }
}

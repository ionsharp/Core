using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Imagin.Common.Extensions;

namespace Imagin.Controls.Common
{
    public class IntUpDown : UpDown
    {
        public int Value
        {
            get
            {
                if (string.IsNullOrEmpty(this.Text))
                    return 0;
                else return this.Text.ToInt();
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

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            Regex r = new Regex("^[0-9]?$");
            e.Handled = !r.IsMatch(e.Text);
        }

        protected override void Up_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.Text))
                this.Text = 0.ToString();
            else this.Text = (this.Text.ToInt() + 1).ToString();
        }

        protected override void Down_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.Text))
                this.Text = 0.ToString();
            else this.Text = (this.Text.ToInt() - 1).ToString();
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

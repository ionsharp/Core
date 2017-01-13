using Imagin.Common.Extensions;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public class RegexBox : RegexBoxBase
    {
        protected override Regex regex
        {
            get
            {
                return new Regex(Pattern);
            }
        }

        public static DependencyProperty PatternProperty = DependencyProperty.Register("Pattern", typeof(string), typeof(RegexBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string Pattern
        {
            get
            {
                return (string)GetValue(PatternProperty);
            }
            set
            {
                SetValue(PatternProperty, value);
            }
        }

        public RegexBox() : base()
        {
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            e.Handled = !Pattern.IsNullOrEmpty() && !regex.IsMatch(e.Text);
        }
    }
}

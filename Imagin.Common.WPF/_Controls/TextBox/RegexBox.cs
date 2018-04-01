using Imagin.Common.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class RegexBox : RegexBoxBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected override Regex regex
        {
            get
            {
                return new Regex(Pattern);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty PatternProperty = DependencyProperty.Register("Pattern", typeof(string), typeof(RegexBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public RegexBox() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            e.Handled = !Pattern.IsNullOrEmpty() && !regex.IsMatch(e.Text);
        }
    }
}

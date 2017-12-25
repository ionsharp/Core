using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class RegexBoxBase : TextBox
    {
        /// <summary>
        /// 
        /// </summary>
        protected virtual Regex regex
        {
            get
            {
                return default(Regex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public RegexBoxBase() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            e.Handled = !regex.IsMatch(e.Text);
        }
    }
}

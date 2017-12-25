using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class IrrationalUpDown<T> : NumericUpDown<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public IrrationalUpDown() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            switch (e.Text)
            {
                case "-":
                    e.Handled = !(CaretIndex == 0 || !Text.Contains("-"));
                    break;
                case ".":
                    e.Handled = !(CaretIndex > 0 || !Text.Contains("."));
                    break;
                default:
                    e.Handled = !Expression.IsMatch(e.Text);
                    break;
            }
        }
    }
}

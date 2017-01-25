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
        public override Regex Expression
        {
            get
            {
                return new Regex("^[0-9]?$");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            e.Handled = CaretIndex > 0 && e.Text == "." && Text.Contains(".");
        }
    }
}

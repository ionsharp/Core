using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public abstract class IrrationalUpDown<T> : NumericUpDown<T>
    {
        public override Regex Expression
        {
            get
            {
                return new Regex("^[0-9]?$");
            }
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e, Regex Expression)
        {
            base.OnPreviewTextInput(e, Expression);
            if (this.CaretIndex > 0 && e.Text == ".")
                e.Handled = this.Text.Contains(".");
        }
    }
}

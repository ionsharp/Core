using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public class AlphaNumericTextBox : TextBox
    {
        Regex Expression
        {
            get
            {
                return new Regex("^[a-zA-Z0-9]*$");
            }
        }

        public AlphaNumericTextBox() : base()
        {
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            Regex r = new Regex("^[a-zA-Z0-9]*$");
            e.Handled = !this.Expression.IsMatch(e.Text);
        }
    }
}

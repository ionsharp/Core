using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public class LongTextBox : TextBox
    {
        public LongTextBox() : base()
        {
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            Regex r = new Regex(@"^\d$");
            e.Handled = !r.IsMatch(e.Text);
        }
    }
}

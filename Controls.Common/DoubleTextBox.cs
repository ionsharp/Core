using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public class DoubleTextBox : TextBox
    {
        public DoubleTextBox() : base()
        {
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            Regex r = new Regex("^[0-9]([.][0-9]{1,3})?$");
            e.Handled = !r.IsMatch(e.Text);
        }
    }
}

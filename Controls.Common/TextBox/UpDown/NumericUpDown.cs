using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public abstract class NumericUpDown : UpDown
    {
        #region Properties

        public abstract Regex Expression
        {
            get;
        }

        #endregion

        #region NumericUpDown

        public NumericUpDown() : base()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prevent invalid character injection.
        /// </summary>
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            //Allow input IFF input is a dash and caret index == 0 OR input is an integer.
            System.Console.WriteLine("\n\n\nThe value is = " + e.Text + " and caret index = " + this.CaretIndex.ToString());
            if (e.Text == "-" && this.CaretIndex == 0)
                e.Handled = false;
            else e.Handled = !this.Expression.IsMatch(e.Text);
        }

        protected override void Trim(string NewText)
        {
            //If text starts with a 0 and isn't 0, remove leading 0s
            if (NewText.StartsWith("0") && NewText != "0")
                base.Trim(NewText.TrimStart('0'));
            //If text starts with a negative 0 and isn't (a negative number less than -1 || a positive number greater than 1)
            else if (NewText.StartsWith("-0") && !NewText.Contains("0."))
                base.Trim(string.Concat("-", NewText.Substring(1).TrimStart('0')));
        }

        #endregion
    }
}

using Imagin.Common.Extensions;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public class AdvancedComboBox : ComboBox
    {
        public event EventHandler<KeyEventArgs> Entered;

        public AdvancedComboBox() : base()
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var PART_TextBox = this.Template.FindName("PART_TextBox", this);
            if (PART_TextBox != null && PART_TextBox.Is<AdvancedTextBox>())
            {
                PART_TextBox.As<AdvancedTextBox>().Entered += (s, e) =>
                {
                    if (this.Entered != null)
                        this.Entered(this, e);
                };
            }
        }
    }
}

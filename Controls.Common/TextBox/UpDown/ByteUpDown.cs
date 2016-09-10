using Imagin.Common.Extensions;
using System;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public class ByteUpDown : RationalUpDown<byte>
    {
        #region ByteUpDown

        public ByteUpDown() : base()
        {
            this.Minimum = Convert.ToByte(0);
            this.Maximum = Convert.ToByte(255);
            this.Value = Convert.ToByte(0);
            this.Increment = Convert.ToByte(1);
        }

        #endregion

        #region Methods

        protected override bool CanIncrease()
        {
            return this.Value < this.Maximum;
        }

        protected override bool CanDecrease()
        {
            return this.Value > this.Minimum;
        }

        protected override object OnMaximumCoerced(object NewMaximum)
        {
            return NewMaximum.As<byte>() < this.Minimum ? this.Minimum : (NewMaximum.As<byte>() > byte.MaxValue ? byte.MaxValue : NewMaximum);
        }

        protected override object OnMinimumCoerced(object NewMinimum)
        {
            return NewMinimum.As<byte>() > this.Maximum ? this.Maximum : (NewMinimum.As<byte>() < byte.MinValue ? byte.MinValue : NewMinimum);
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e, Regex Expression)
        {
            base.OnPreviewTextInput(e, Expression);
            e.Handled = this.CaretIndex > 0 && e.Text == "-" ? true : e.Handled;
        }

        protected override bool OnTextChanged()
        {
            if (!base.OnTextChanged()) return false;
            this.Value = this.Text.ToByte();
            return true;
        }

        protected override bool OnValueChanged()
        {
            if (!base.OnValueChanged()) return false;
            this.SetText(this.Value.ToString(this.StringFormat));
            return true;
        }

        protected override object OnValueCoerced(object NewValue)
        {
            return NewValue.As<byte>() < this.Minimum ? this.Minimum : (NewValue.As<byte>() > this.Maximum ? this.Maximum : NewValue);
        }

        public override void Increase()
        {
            this.Value += this.Increment;
        }

        public override void Decrease()
        {
            this.Value -= this.Increment;
        }

        #endregion
    }
}

using Imagin.Common.Extensions;
using System;

namespace Imagin.Controls.Common
{
    public class IntUpDown : RationalUpDown<int>
    {
        #region IntUpDown

        public IntUpDown() : base()
        {
            this.Minimum = -1000000;
            this.Maximum = 1000000;
            this.Value = 0;
            this.Increment = 1;
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
            return NewMaximum.As<int>() < this.Minimum ? this.Minimum : (NewMaximum.As<int>() > int.MaxValue ? int.MaxValue : NewMaximum);
        }

        protected override object OnMinimumCoerced(object NewMinimum)
        {
            return NewMinimum.As<int>() > this.Maximum ? this.Maximum : (NewMinimum.As<int>() < int.MinValue ? int.MinValue : NewMinimum);
        }

        protected override bool OnTextChanged()
        {
            if (!base.OnTextChanged()) return false;
            this.Value = this.OnValueCoerced(this.Text.ToInt()).As<int>();
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
            return NewValue.As<int>() < this.Minimum ? this.Minimum : (NewValue.As<int>() > this.Maximum ? this.Maximum : NewValue);
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

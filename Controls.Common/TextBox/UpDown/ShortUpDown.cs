using System;
using Imagin.Common.Extensions;

namespace Imagin.Controls.Common
{
    public class ShortUpDown : RationalUpDown<short>
    {
        #region ShortUpDown

        public ShortUpDown() : base()
        {
            this.Value = Convert.ToInt16(0);
            this.Minimum = Convert.ToInt16(-10000);
            this.Maximum = Convert.ToInt16(10000);
            this.Increment = Convert.ToInt16(1);
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
            return NewMaximum.As<short>() < this.Minimum ? this.Minimum : (NewMaximum.As<short>() > short.MaxValue ? short.MaxValue : NewMaximum);
        }

        protected override object OnMinimumCoerced(object NewMinimum)
        {
            return NewMinimum.As<short>() > this.Maximum ? this.Maximum : (NewMinimum.As<short>() < short.MinValue ? short.MinValue : NewMinimum);
        }

        protected override bool OnTextChanged()
        {
            if (!base.OnTextChanged()) return false;
            this.Value = this.OnValueCoerced(this.Text.ToShort()).As<short>();
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
            return NewValue.As<short>() < this.Minimum ? this.Minimum : (NewValue.As<short>() > this.Maximum ? this.Maximum : NewValue);
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

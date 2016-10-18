using Imagin.Common.Extensions;

namespace Imagin.Controls.Common
{
    public class LongUpDown : RationalUpDown<long>
    {
        #region LongUpDown

        public LongUpDown() : base()
        {
            this.Increment = 1L;
            this.Minimum = long.MinValue;
            this.Maximum = long.MaxValue;
            this.Value = 0L;
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
            return NewMaximum.As<long>() < this.Minimum ? this.Minimum : (NewMaximum.As<long>() > long.MaxValue ? long.MaxValue : NewMaximum);
        }

        protected override object OnMinimumCoerced(object NewMinimum)
        {
            return NewMinimum.As<long>() > this.Maximum ? this.Maximum : (NewMinimum.As<long>() < long.MinValue ? long.MinValue : NewMinimum);
        }

        protected override bool OnTextChanged()
        {
            if (!base.OnTextChanged()) return false;
            this.Value = this.OnValueCoerced(this.Text.ToLong()).As<long>();
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
            return NewValue.As<long>() < this.Minimum ? this.Minimum : (NewValue.As<long>() > this.Maximum ? this.Maximum : NewValue);
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

using Imagin.Common.Extensions;

namespace Imagin.Controls.Common
{
    public class DecimalUpDown : IrrationalUpDown<decimal>
    {
        #region DecimalUpDown

        public DecimalUpDown() : base()
        {
            this.Minimum = -1000000m;
            this.Maximum = 1000000m;
            this.Value = 0m;
            this.Increment = 1m;
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
            return NewMaximum.As<decimal>() < this.Minimum ? this.Minimum : (NewMaximum.As<decimal>() > decimal.MaxValue ? decimal.MaxValue : NewMaximum);
        }

        protected override object OnMinimumCoerced(object NewMinimum)
        {
            return NewMinimum.As<decimal>() > this.Maximum ? this.Maximum : (NewMinimum.As<decimal>() < decimal.MinValue ? decimal.MinValue : NewMinimum);
        }

        protected override bool OnTextChanged()
        {
            if (!base.OnTextChanged()) return false;
            this.Value = this.OnValueCoerced(this.Text.ToDecimal()).As<decimal>();
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
            return NewValue.As<decimal>() < this.Minimum ? this.Minimum : (NewValue.As<decimal>() > this.Maximum ? this.Maximum : NewValue);
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

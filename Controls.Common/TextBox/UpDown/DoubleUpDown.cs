using Imagin.Common.Extensions;

namespace Imagin.Controls.Common
{
    public class DoubleUpDown : IrrationalUpDown<double>
    {
        #region DoubleUpDown

        public DoubleUpDown() : base()
        {
            this.Minimum = -1000000.0;
            this.Maximum = 1000000.0;
            this.Value = 0.0;
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
            return NewMaximum.As<double>() < this.Minimum ? this.Minimum : (NewMaximum.As<double>() > double.MaxValue ? double.MaxValue : NewMaximum);
        }

        protected override object OnMinimumCoerced(object NewMinimum)
        {
            return NewMinimum.As<double>() > this.Maximum ? this.Maximum : (NewMinimum.As<double>() < double.MinValue ? double.MinValue : NewMinimum);
        }

        protected override bool OnTextChanged()
        {
            if (!base.OnTextChanged()) return false;
            this.Value = this.OnValueCoerced(this.Text.ToDouble()).As<double>();
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
            return NewValue.As<double>() < this.Minimum ? this.Minimum : (NewValue.As<double>() > this.Maximum ? this.Maximum : NewValue);
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

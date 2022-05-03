using Imagin.Common.Linq;
using System;

namespace Imagin.Common.Controls
{
    public class UInt64UpDown : NumericUpDown<ulong>
    {
        public override ulong AbsoluteMaximum => ulong.MaxValue;

        public override ulong AbsoluteMinimum => ulong.MinValue;

        public override ulong DefaultIncrement => 1;

        public override ulong DefaultValue => 0;

        public override bool IsRational => true;

        public override bool IsSigned => false;

        public UInt64UpDown() : base() { }

        protected override ulong GetValue(string input) => input.UInt64();

        protected override string ToString(ulong input) => input.ToString(StringFormat);

        protected override bool CanIncrease() => Value < Maximum;

        protected override bool CanDecrease() => Value > Minimum;

        protected override object OnMaximumCoerced(object input) => input.As<ulong>().Coerce(AbsoluteMaximum, this.Value);

        protected override object OnMinimumCoerced(object input) => input.As<ulong>().Coerce(this.Value, AbsoluteMinimum);

        protected override object OnValueCoerced(object input) => input.As<ulong>().Coerce(Maximum, Minimum);

        public override void Increase() => SetCurrentValue(ValueProperty.Property, Convert.ToUInt64(Value + Increment));

        public override void Decrease() => SetCurrentValue(ValueProperty.Property, Convert.ToUInt64(Value - Increment));
    }
}
using Imagin.Common.Linq;

namespace Imagin.Common.Controls
{
    public class Int64UpDown : NumericUpDown<long>
    {
        public override long AbsoluteMaximum => long.MaxValue;

        public override long AbsoluteMinimum => long.MinValue;

        public override long DefaultIncrement => 1L;

        public override long DefaultValue => 0L;

        public override bool IsRational => true;

        public override bool IsSigned => true;

        public Int64UpDown() : base() { }

        protected override long GetValue(string input) => input.Int64();

        protected override string ToString(long input) => input.ToString(StringFormat);

        protected override bool CanIncrease() => Value < Maximum;

        protected override bool CanDecrease() => Value > Minimum;

        protected override object OnMaximumCoerced(object input) => input.As<long>().Coerce(AbsoluteMaximum, this.Value);

        protected override object OnMinimumCoerced(object input) => input.As<long>().Coerce(this.Value, AbsoluteMinimum);

        protected override object OnValueCoerced(object input) => input.As<long>().Coerce(Maximum, Minimum);

        public override void Increase() => SetCurrentValue(ValueProperty.Property, Value + Increment);

        public override void Decrease() => SetCurrentValue(ValueProperty.Property, Value - Increment);
    }
}
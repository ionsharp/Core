using Imagin.Common.Linq;

namespace Imagin.Common.Controls
{
    public class DecimalUpDown : NumericUpDown<decimal>
    {
        public override decimal AbsoluteMaximum => decimal.MaxValue;

        public override decimal AbsoluteMinimum => decimal.MinValue;

        public override decimal DefaultIncrement => 1m;

        public override decimal DefaultValue => 0m;

        public override bool IsRational => false;

        public override bool IsSigned => true;

        public DecimalUpDown() : base() => Increment = 1m;

        protected override decimal GetValue(string input) => input.Decimal();

        protected override string ToString(decimal input) => input.ToString(StringFormat);

        protected override bool CanIncrease() => Value < Maximum;

        protected override bool CanDecrease() => Value > Minimum;

        protected override object OnMaximumCoerced(object input) => input.As<decimal>().Coerce(AbsoluteMaximum, this.Value);

        protected override object OnMinimumCoerced(object input) => input.As<decimal>().Coerce(this.Value, AbsoluteMinimum);

        protected override object OnValueCoerced(object input) => input.As<decimal>().Coerce(Maximum, Minimum);

        public override void Increase() => SetCurrentValue(ValueProperty.Property, Value + Increment);

        public override void Decrease() => SetCurrentValue(ValueProperty.Property, Value - Increment);
    }
}
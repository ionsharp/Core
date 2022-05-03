using Imagin.Common.Linq;

namespace Imagin.Common.Controls
{
    public class UDoubleUpDown : NumericUpDown<UDouble>
    {
        public override UDouble AbsoluteMaximum => UDouble.MaxValue;

        public override UDouble AbsoluteMinimum => UDouble.MinValue;

        public override UDouble DefaultIncrement => 1;

        public override UDouble DefaultValue => 0;

        public override bool IsRational => false;

        public override bool IsSigned => false;

        public UDoubleUpDown() : base() { }

        protected override UDouble GetValue(string input) => input.UDouble();

        protected override string ToString(UDouble input) => input.ToString(StringFormat);

        protected override bool CanIncrease() => Value < Maximum;

        protected override bool CanDecrease() => Value > Minimum;

        protected override object OnMaximumCoerced(object input) => input.As<UDouble>().Coerce(AbsoluteMaximum, Value);

        protected override object OnMinimumCoerced(object input) => input.As<UDouble>().Coerce(Value, AbsoluteMinimum);

        protected override object OnValueCoerced(object input) => input.As<UDouble>().Coerce(Maximum, Minimum);

        public override void Increase() => SetCurrentValue(ValueProperty.Property, Value + Increment);

        public override void Decrease() => SetCurrentValue(ValueProperty.Property, Value - Increment);
    }
}
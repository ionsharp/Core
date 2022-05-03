using Imagin.Common.Linq;

namespace Imagin.Common.Controls
{
    public class SingleUpDown : NumericUpDown<float>
    {
        public override float AbsoluteMaximum => float.MaxValue;

        public override float AbsoluteMinimum => float.MinValue;

        public override float DefaultIncrement => 1f;

        public override float DefaultValue => 0f;

        public override bool IsRational => false;

        public override bool IsSigned => true;

        public SingleUpDown() : base() { }

        protected override float GetValue(string input) => input.Single();

        protected override string ToString(float input) => input.ToString(StringFormat);

        protected override bool CanIncrease() => Value < Maximum;

        protected override bool CanDecrease() => Value > Minimum;

        protected override object OnMaximumCoerced(object input) => input.As<float>().Coerce(AbsoluteMaximum, Value);

        protected override object OnMinimumCoerced(object input) => input.As<float>().Coerce(Value, AbsoluteMinimum);

        protected override object OnValueCoerced(object input) => input.As<float>().Coerce(Maximum, Minimum);

        public override void Increase() => SetCurrentValue(ValueProperty.Property, Value + Increment);

        public override void Decrease() => SetCurrentValue(ValueProperty.Property, Value - Increment);
    }
}
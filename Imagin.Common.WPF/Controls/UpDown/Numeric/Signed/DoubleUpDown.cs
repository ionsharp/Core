using Imagin.Common.Linq;

namespace Imagin.Common.Controls
{
    public class DoubleUpDown : NumericUpDown<double>
    {
        public override double AbsoluteMaximum => double.MaxValue;

        public override double AbsoluteMinimum => double.MinValue;

        public override double DefaultIncrement => 1;

        public override double DefaultValue => 0;

        public override bool IsRational => false;

        public override bool IsSigned => true;

        public DoubleUpDown() : base() { }

        protected override double GetValue(string input) => input.Double();

        protected override string ToString(double input) => input.ToString(StringFormat);

        protected override bool CanIncrease() => Value < Maximum;

        protected override bool CanDecrease() => Value > Minimum;

        protected override object OnMaximumCoerced(object input) => input.As<double>().Coerce(AbsoluteMaximum, Value);

        protected override object OnMinimumCoerced(object input) => input.As<double>().Coerce(Value, AbsoluteMinimum);

        protected override object OnValueCoerced(object input) => input.As<double>().Coerce(Maximum, Minimum);

        public override void Increase() => SetCurrentValue(ValueProperty.Property, Value + Increment);

        public override void Decrease() => SetCurrentValue(ValueProperty.Property, Value - Increment);
    }
}
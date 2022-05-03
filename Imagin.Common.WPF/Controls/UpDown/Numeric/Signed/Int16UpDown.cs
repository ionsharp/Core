using Imagin.Common.Linq;
using System;

namespace Imagin.Common.Controls
{
    public class Int16UpDown : NumericUpDown<short>
    {
        public override short AbsoluteMaximum => short.MaxValue;

        public override short AbsoluteMinimum => short.MinValue;

        public override short DefaultIncrement => 1;

        public override short DefaultValue => Convert.ToInt16(0);

        public override bool IsRational => true;

        public override bool IsSigned => true;

        public Int16UpDown() : base() { }

        protected override short GetValue(string input) => input.Int16();

        protected override string ToString(short input) => input.ToString(StringFormat);

        protected override bool CanIncrease() => Value < Maximum;

        protected override bool CanDecrease() => Value > Minimum;

        protected override object OnMaximumCoerced(object input) => input.As<short>().Coerce(AbsoluteMaximum, this.Value);

        protected override object OnMinimumCoerced(object input) => input.As<short>().Coerce(this.Value, AbsoluteMinimum);

        protected override object OnValueCoerced(object input) => input.As<short>().Coerce(Maximum, Minimum);

        public override void Increase() => SetCurrentValue(ValueProperty.Property, (Value + Increment).Int16());

        public override void Decrease() => SetCurrentValue(ValueProperty.Property, (Value - Increment).Int16());
    }
}
using Imagin.Common.Extensions;

namespace Imagin.Controls.Common
{
    public class DecimalUpDown : IrrationalUpDown<decimal>
    {
        public override decimal AbsoluteMaximum
        {
            get
            {
                return decimal.MaxValue;
            }
        }

        public override decimal AbsoluteMinimum
        {
            get
            {
                return decimal.MinValue;
            }
        }

        public override decimal DefaultIncrement
        {
            get
            {
                return 1m;
            }
        }

        public override decimal DefaultValue
        {
            get
            {
                return 0m;
            }
        }

        public DecimalUpDown() : base()
        {
            Increment = 1m;
        }

        protected override decimal GetValue(string Value)
        {
            return Value.ToDecimal();
        }

        protected override string ToString(decimal Value)
        {
            return Value.ToString(StringFormat);
        }

        protected override bool CanIncrease()
        {
            return Value < Maximum;
        }

        protected override bool CanDecrease()
        {
            return Value > Minimum;
        }

        protected override object OnMaximumCoerced(object Value)
        {
            return Value.As<decimal>().Coerce(AbsoluteMaximum, this.Value);
        }

        protected override object OnMinimumCoerced(object Value)
        {
            return Value.As<decimal>().Coerce(this.Value, AbsoluteMinimum);
        }

        protected override object OnValueCoerced(object Value)
        {
            return Value.As<decimal>().Coerce(Maximum, Minimum);
        }

        public override void Increase()
        {
            Value += Increment;
        }

        public override void Decrease()
        {
            Value -= Increment;
        }
    }
}

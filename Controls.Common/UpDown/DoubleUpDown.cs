using Imagin.Common.Extensions;

namespace Imagin.Controls.Common
{
    public class DoubleUpDown : IrrationalUpDown<double>
    {
        public override double AbsoluteMaximum
        {
            get
            {
                return double.MaxValue;
            }
        }

        public override double AbsoluteMinimum
        {
            get
            {
                return double.MinValue;
            }
        }

        public override double DefaultIncrement
        {
            get
            {
                return 1d;
            }
        }

        public override double DefaultValue
        {
            get
            {
                return 0d;
            }
        }

        public DoubleUpDown() : base()
        {
        }

        protected override double GetValue(string Value)
        {
            return Value.ToDouble();
        }

        protected override string ToString(double Value)
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
            return Value.As<double>().Coerce(AbsoluteMaximum, this.Value);
        }

        protected override object OnMinimumCoerced(object Value)
        {
            return Value.As<double>().Coerce(this.Value, AbsoluteMinimum);
        }

        protected override object OnValueCoerced(object Value)
        {
            return Value.As<double>().Coerce(Maximum, Minimum);
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

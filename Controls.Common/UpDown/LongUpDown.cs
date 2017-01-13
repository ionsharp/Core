using Imagin.Common.Extensions;

namespace Imagin.Controls.Common
{
    public class LongUpDown : RationalUpDown<long>
    {
        public override long AbsoluteMaximum
        {
            get
            {
                return long.MaxValue;
            }
        }

        public override long AbsoluteMinimum
        {
            get
            {
                return long.MinValue;
            }
        }

        public override long DefaultIncrement
        {
            get
            {
                return 1L;
            }
        }

        public override long DefaultValue
        {
            get
            {
                return 0L;
            }
        }

        public LongUpDown() : base()
        {
        }

        protected override long GetValue(string Value)
        {
            return Value.ToInt64();
        }

        protected override string ToString(long Value)
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
            return Value.As<long>().Coerce(AbsoluteMaximum, this.Value);
        }

        protected override object OnMinimumCoerced(object Value)
        {
            return Value.As<long>().Coerce(this.Value, AbsoluteMinimum);
        }

        protected override object OnValueCoerced(object Value)
        {
            return Value.As<long>().Coerce(Maximum, Minimum);
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

using System;
using Imagin.Common.Extensions;

namespace Imagin.Controls.Common
{
    public class ShortUpDown : RationalUpDown<short>
    {
        public override short AbsoluteMaximum
        {
            get
            {
                return short.MaxValue;
            }
        }

        public override short AbsoluteMinimum
        {
            get
            {
                return short.MinValue;
            }
        }

        public override short DefaultIncrement
        {
            get
            {
                return (short)1;
            }
        }

        public override short DefaultValue
        {
            get
            {
                return Convert.ToInt16(0);
            }
        }

        public ShortUpDown() : base()
        {
        }

        protected override short GetValue(string Value)
        {
            return Value.ToInt16();
        }

        protected override string ToString(short Value)
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
            return Value.As<short>().Coerce(AbsoluteMaximum, this.Value);
        }

        protected override object OnMinimumCoerced(object Value)
        {
            return Value.As<short>().Coerce(this.Value, AbsoluteMinimum);
        }

        protected override object OnValueCoerced(object Value)
        {
            return Value.As<short>().Coerce(Maximum, Minimum);
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

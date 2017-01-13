using Imagin.Common.Extensions;
using System;

namespace Imagin.Controls.Common
{
    public class IntUpDown : RationalUpDown<int>
    {
        public override int AbsoluteMaximum
        {
            get
            {
                return int.MaxValue;
            }
        }

        public override int AbsoluteMinimum
        {
            get
            {
                return int.MinValue;
            }
        }

        public override int DefaultIncrement
        {
            get
            {
                return 1;
            }
        }

        public override int DefaultValue
        {
            get
            {
                return 0;
            }
        }

        public IntUpDown() : base()
        {
        }

        protected override int GetValue(string Value)
        {
            return Value.ToInt32();
        }

        protected override string ToString(int Value)
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
            return Value.As<int>().Coerce(AbsoluteMaximum, this.Value);
        }

        protected override object OnMinimumCoerced(object Value)
        {
            return Value.As<int>().Coerce(this.Value, AbsoluteMinimum);
        }

        protected override object OnValueCoerced(object Value)
        {
            return Value.As<int>().Coerce(Maximum, Minimum);
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

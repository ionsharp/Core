using Imagin.Common.Extensions;
using System;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public class ByteUpDown : RationalUpDown<byte>
    {
        public override byte AbsoluteMaximum
        {
            get
            {
                return byte.MaxValue;
            }
        }

        public override byte AbsoluteMinimum
        {
            get
            {
                return byte.MinValue;
            }
        }

        public override byte DefaultIncrement
        {
            get
            {
                return (byte)1;
            }
        }

        public override byte DefaultValue
        {
            get
            {
                return (byte)0;
            }
        }

        public ByteUpDown() : base()
        {
        }

        protected override byte GetValue(string Value)
        {
            return Value.ToByte();
        }

        protected override string ToString(byte Value)
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
            return Value.As<byte>().Coerce(AbsoluteMaximum, this.Value);
        }

        protected override object OnMinimumCoerced(object Value)
        {
            return Value.As<byte>().Coerce(this.Value, AbsoluteMinimum);
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e, Regex Expression)
        {
            base.OnPreviewTextInput(e, Expression);
            e.Handled = CaretIndex > 0 && e.Text == "-" ? true : e.Handled;
        }

        protected override object OnValueCoerced(object Value)
        {
            return Value.As<byte>().Coerce(Maximum, Minimum);
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

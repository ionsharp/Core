using System;
using Imagin.Common.Linq;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class UInt16UpDown : UnsignedRationalUpDown<ushort>
    {
        /// <summary>
        /// 
        /// </summary>
        public override ushort AbsoluteMaximum
        {
            get
            {
                return ushort.MaxValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override ushort AbsoluteMinimum
        {
            get
            {
                return ushort.MinValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override ushort DefaultIncrement
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override ushort DefaultValue
        {
            get
            {
                return Convert.ToUInt16(0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UInt16UpDown() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override ushort GetValue(string Value)
        {
            return Value.ToUInt16();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override string ToString(ushort Value)
        {
            return Value.ToString(StringFormat);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool CanIncrease()
        {
            return Value < Maximum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool CanDecrease()
        {
            return Value > Minimum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override object OnMaximumCoerced(object value)
        {
            return value.As<ushort>().Coerce(AbsoluteMaximum, Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override object OnMinimumCoerced(object value)
        {
            return value.As<ushort>().Coerce(Value, AbsoluteMinimum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override object OnValueCoerced(object value)
        {
            return value.As<ushort>().Coerce(Maximum, Minimum);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Increase()
        {
            SetCurrentValue(ValueProperty.Property, Convert.ToUInt16(Value + Increment));
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Decrease()
        {
            SetCurrentValue(ValueProperty.Property, Convert.ToUInt16(Value - Increment));
        }
    }
}

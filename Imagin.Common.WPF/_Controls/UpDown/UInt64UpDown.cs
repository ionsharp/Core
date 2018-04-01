using Imagin.Common.Linq;
using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class UInt64UpDown : UnsignedRationalUpDown<ulong>
    {
        /// <summary>
        /// 
        /// </summary>
        public override ulong AbsoluteMaximum
        {
            get
            {
                return ulong.MaxValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override ulong AbsoluteMinimum
        {
            get
            {
                return ulong.MinValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override ulong DefaultIncrement
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override ulong DefaultValue
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UInt64UpDown() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override ulong GetValue(string Value)
        {
            return Value.ToUInt64();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override string ToString(ulong Value)
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
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override object OnMaximumCoerced(object Value)
        {
            return Value.As<ulong>().Coerce(AbsoluteMaximum, this.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override object OnMinimumCoerced(object Value)
        {
            return Value.As<ulong>().Coerce(this.Value, AbsoluteMinimum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override object OnValueCoerced(object Value)
        {
            return Value.As<ulong>().Coerce(Maximum, Minimum);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Increase()
        {
            SetCurrentValue(ValueProperty.Property, Convert.ToUInt64(Value + Increment));
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Decrease()
        {
            SetCurrentValue(ValueProperty.Property, Convert.ToUInt64(Value - Increment));
        }
    }
}

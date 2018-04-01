using Imagin.Common.Linq;
using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class UInt32UpDown : UnsignedRationalUpDown<uint>
    {
        /// <summary>
        /// 
        /// </summary>
        public override uint AbsoluteMaximum
        {
            get
            {
                return uint.MaxValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override uint AbsoluteMinimum
        {
            get
            {
                return uint.MinValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override uint DefaultIncrement
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override uint DefaultValue
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UInt32UpDown() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override uint GetValue(string Value)
        {
            return Value.ToUInt32();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override string ToString(uint Value)
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
            return Value.As<uint>().Coerce(AbsoluteMaximum, this.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override object OnMinimumCoerced(object Value)
        {
            return Value.As<uint>().Coerce(this.Value, AbsoluteMinimum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override object OnValueCoerced(object Value)
        {
            return Value.As<uint>().Coerce(Maximum, Minimum);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Increase()
        {
            SetCurrentValue(ValueProperty.Property, Convert.ToUInt32(Value + Increment));
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Decrease()
        {
            SetCurrentValue(ValueProperty.Property, Convert.ToUInt32(Value - Increment));
        }
    }
}

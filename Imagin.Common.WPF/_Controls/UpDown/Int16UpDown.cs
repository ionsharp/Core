using System;
using Imagin.Common.Linq;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class Int16UpDown : SignedRationalUpDown<short>
    {
        /// <summary>
        /// 
        /// </summary>
        public override short AbsoluteMaximum
        {
            get
            {
                return short.MaxValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override short AbsoluteMinimum
        {
            get
            {
                return short.MinValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override short DefaultIncrement
        {
            get
            {
                return (short)1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override short DefaultValue
        {
            get
            {
                return Convert.ToInt16(0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int16UpDown() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override short GetValue(string Value)
        {
            return Value.ToInt16();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override string ToString(short Value)
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
            return Value.As<short>().Coerce(AbsoluteMaximum, this.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override object OnMinimumCoerced(object Value)
        {
            return Value.As<short>().Coerce(this.Value, AbsoluteMinimum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override object OnValueCoerced(object Value)
        {
            return Value.As<short>().Coerce(Maximum, Minimum);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Increase()
        {
            SetCurrentValue(ValueProperty.Property, (Value + Increment).ToInt16());
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Decrease()
        {
            SetCurrentValue(ValueProperty.Property, (Value - Increment).ToInt16());
        }
    }
}

using Imagin.Common;
using Imagin.Common.Linq;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class UDoubleUpDown : UnsignedRationalUpDown<UDouble>
    {
        /// <summary>
        /// 
        /// </summary>
        public override UDouble AbsoluteMaximum
        {
            get
            {
                return UDouble.Maximum;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override UDouble AbsoluteMinimum
        {
            get
            {
                return UDouble.Minimum;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override UDouble DefaultIncrement
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override UDouble DefaultValue
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UDoubleUpDown() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override UDouble GetValue(string Value)
        {
            return Value.ToUDouble();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override string ToString(UDouble Value)
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
            return Value.As<UDouble>().Coerce(AbsoluteMaximum, this.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override object OnMinimumCoerced(object Value)
        {
            return Value.As<UDouble>().Coerce(this.Value, AbsoluteMinimum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override object OnValueCoerced(object Value)
        {
            return Value.As<UDouble>().Coerce(Maximum, Minimum);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Increase()
        {
            SetCurrentValue(ValueProperty.Property, Value + Increment);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Decrease()
        {
            SetCurrentValue(ValueProperty.Property, Value - Increment);
        }
    }
}

using Imagin.Common.Linq;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class DecimalUpDown : IrrationalUpDown<decimal>
    {
        /// <summary>
        /// 
        /// </summary>
        public override decimal AbsoluteMaximum
        {
            get
            {
                return decimal.MaxValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override decimal AbsoluteMinimum
        {
            get
            {
                return decimal.MinValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override decimal DefaultIncrement
        {
            get
            {
                return 1m;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override decimal DefaultValue
        {
            get
            {
                return 0m;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DecimalUpDown() : base()
        {
            Increment = 1m;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override decimal GetValue(string Value)
        {
            return Value.ToDecimal();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override string ToString(decimal Value)
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
            return Value.As<decimal>().Coerce(AbsoluteMaximum, this.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override object OnMinimumCoerced(object Value)
        {
            return Value.As<decimal>().Coerce(this.Value, AbsoluteMinimum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override object OnValueCoerced(object Value)
        {
            return Value.As<decimal>().Coerce(Maximum, Minimum);
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

using Imagin.Common.Linq;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class Int64UpDown : SignedRationalUpDown<long>
    {
        /// <summary>
        /// 
        /// </summary>
        public override long AbsoluteMaximum
        {
            get
            {
                return long.MaxValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override long AbsoluteMinimum
        {
            get
            {
                return long.MinValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override long DefaultIncrement
        {
            get
            {
                return 1L;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override long DefaultValue
        {
            get
            {
                return 0L;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int64UpDown() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override long GetValue(string Value)
        {
            return Value.ToInt64();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override string ToString(long Value)
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
            return Value.As<long>().Coerce(AbsoluteMaximum, this.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override object OnMinimumCoerced(object Value)
        {
            return Value.As<long>().Coerce(this.Value, AbsoluteMinimum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override object OnValueCoerced(object Value)
        {
            return Value.As<long>().Coerce(Maximum, Minimum);
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

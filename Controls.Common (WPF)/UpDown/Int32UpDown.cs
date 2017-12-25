using Imagin.Common.Linq;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class Int32UpDown : SignedRationalUpDown<int>
    {
        /// <summary>
        /// 
        /// </summary>
        public override int AbsoluteMaximum
        {
            get
            {
                return int.MaxValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int AbsoluteMinimum
        {
            get
            {
                return int.MinValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int DefaultIncrement
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int DefaultValue
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32UpDown() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override int GetValue(string Value)
        {
            return Value.ToInt32();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override string ToString(int Value)
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
            return Value.As<int>().Coerce(AbsoluteMaximum, this.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override object OnMinimumCoerced(object Value)
        {
            return Value.As<int>().Coerce(this.Value, AbsoluteMinimum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override object OnValueCoerced(object Value)
        {
            return Value.As<int>().Coerce(Maximum, Minimum);
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

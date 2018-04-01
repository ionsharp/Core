using Imagin.Common.Linq;
using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class FloatUpDown : IrrationalUpDown<float>
    {
        /// <summary>
        /// 
        /// </summary>
        public override float AbsoluteMaximum
        {
            get
            {
                return float.MaxValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override float AbsoluteMinimum
        {
            get
            {
                return float.MinValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override float DefaultIncrement
        {
            get
            {
                return 1f;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override float DefaultValue
        {
            get
            {
                return 0f;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public FloatUpDown() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override float GetValue(string Value)
        {
            return Value.ToFloat();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override string ToString(float Value)
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
            return value.As<float>().Coerce(AbsoluteMaximum, Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override object OnMinimumCoerced(object value)
        {
            return value.As<float>().Coerce(Value, AbsoluteMinimum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override object OnValueCoerced(object value)
        {
            return value.As<float>().Coerce(Maximum, Minimum);
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

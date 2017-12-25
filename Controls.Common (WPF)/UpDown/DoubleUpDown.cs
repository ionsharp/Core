using Imagin.Common.Linq;
using System;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class DoubleUpDown : IrrationalUpDown<double>
    {
        /// <summary>
        /// 
        /// </summary>
        public override double AbsoluteMaximum
        {
            get
            {
                return double.MaxValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override double AbsoluteMinimum
        {
            get
            {
                return double.MinValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override double DefaultIncrement
        {
            get
            {
                return 1d;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override double DefaultValue
        {
            get
            {
                return 0d;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DoubleUpDown() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override double GetValue(string Value)
        {
            return Value.ToDouble();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override string ToString(double Value)
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
            return Value.As<double>().Coerce(AbsoluteMaximum, this.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override object OnMinimumCoerced(object Value)
        {
            return Value.As<double>().Coerce(this.Value, AbsoluteMinimum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override object OnValueCoerced(object Value)
        {
            return Value.As<double>().Coerce(Maximum, Minimum);
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

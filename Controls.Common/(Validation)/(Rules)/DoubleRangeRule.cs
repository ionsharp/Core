using Imagin.Common.Extensions;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class DoubleRangeRule : RangeRule<double>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override double DefaultMax
        {
            get
            {
                return double.MaxValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override double DefaultMin
        {
            get
            {
                return double.MinValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected override ValidationResult Validate(object Value)
        {
            var value = Value.ToString().ToInt16();

            if (value > Max) return ValidateMax();
            if (value < Min) return ValidateMin();

            return null;
        }
    }
}

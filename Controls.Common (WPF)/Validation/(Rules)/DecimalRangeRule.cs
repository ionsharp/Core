using Imagin.Common.Linq;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class DecimalRangeRule : RangeRule<decimal>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override decimal DefaultMax
        {
            get
            {
                return decimal.MaxValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override decimal DefaultMin
        {
            get
            {
                return decimal.MinValue;
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

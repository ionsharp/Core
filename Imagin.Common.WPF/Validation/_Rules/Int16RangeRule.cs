using Imagin.Common.Linq;
using System.Windows.Controls;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class Int16RangeRule : RangeRule<short>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override short DefaultMax => short.MaxValue;

        /// <summary>
        /// 
        /// </summary>
        protected override short DefaultMin => short.MinValue;

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

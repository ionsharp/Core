using Imagin.Common.Linq;
using System.Windows.Controls;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class Int32RangeRule : RangeRule<int>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override int DefaultMax => int.MaxValue;

        /// <summary>
        /// 
        /// </summary>
        protected override int DefaultMin => int.MinValue;

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

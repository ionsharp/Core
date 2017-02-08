using Imagin.Common.Extensions;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class Int64RangeRule : RangeRule<long>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override long DefaultMax
        {
            get
            {
                return long.MaxValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override long DefaultMin
        {
            get
            {
                return long.MinValue;
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

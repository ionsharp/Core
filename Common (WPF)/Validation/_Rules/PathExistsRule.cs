using Imagin.Common.Linq;
using System;
using System.Windows.Controls;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class PathExistsRule : NamedRule
    {
        PathValidateHandler handler;
        /// <summary>
        /// A handler used to validate the path.
        /// </summary>
        public PathValidateHandler Handler
        {
            get => handler;
            set => handler = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value?.ToString().IsNullOrEmpty() == false && Handler != null)
            {
                Name = Name.Length == 0 ? "Field" : Name;

                if (!Handler.Validate(value.ToString()))
                    return new ValidationResult(false, "{0} does not exist.".F(Name));
            }
            return ValidationResult.ValidResult;
        }
    }
}

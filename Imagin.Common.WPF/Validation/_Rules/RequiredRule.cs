using System;
using System.Windows.Controls;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class RequiredRule : ValidationRule
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (String.IsNullOrEmpty((string)value))
            {
                if (Name.Length == 0)
                    Name = "Field";
                return new ValidationResult(false, Name + " is mandatory.");
            }
            return ValidationResult.ValidResult;
        }
    }
}

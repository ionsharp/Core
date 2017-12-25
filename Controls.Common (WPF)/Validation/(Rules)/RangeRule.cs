using System;
using System.Windows.Controls;
using Imagin.Common.Linq;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class RangeRule<TValue> : NamedRule
    {
        /// <summary>
        /// 
        /// </summary>
        protected abstract TValue DefaultMax
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract TValue DefaultMin
        {
            get;
        }

        TValue max = default(TValue);
        /// <summary>
        /// 
        /// </summary>
        public TValue Max
        {
            get
            {
                return max;
            }
            set
            {
                max = value;
            }
        }

        TValue min = default(TValue);
        /// <summary>
        /// 
        /// </summary>
        public TValue Min
        {
            get
            {
                return min;
            }
            set
            {
                min = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public RangeRule() : base()
        {
            max = DefaultMax;
            min = DefaultMin;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected ValidationResult ValidateMax()
        {
            return new ValidationResult(false, Name + " must be <= " + Max + ".");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected ValidationResult ValidateMin()
        {
            return new ValidationResult(false, Name + " must be >= " + Min + ".");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected abstract ValidationResult Validate(object Value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value?.ToString().IsNullOrEmpty() == false)
            {
                Name = Name.Length == 0 ? "Field" : Name;

                try
                {
                    if (((string)value).Length > 0)
                    {
                        var Result = Validate(value);

                        if (Result != null)
                            return Result;
                    }
                }
                catch (Exception)
                {
                    //Try to match the system generated error message so it does not look out of place.
                    return new ValidationResult(false, Name + " is not in a correct numeric format.");
                }
            }
            return ValidationResult.ValidResult;
        }
    }
}


using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class AbbreviationAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public AbbreviationAttribute(string value)
        {
            Value = value;
        }
    }
}

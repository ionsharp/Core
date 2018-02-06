using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class AbbreviationAttribute : Attribute
    {
        readonly string value;
        /// <summary>
        /// 
        /// </summary>
        public string Value
        {
            get => value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public AbbreviationAttribute(string Value) : base()
        {
            value = Value;
        }
    }
}

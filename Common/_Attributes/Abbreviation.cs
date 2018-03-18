using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class AbbreviationAttribute : Attribute
    {
        readonly string _value;
        /// <summary>
        /// 
        /// </summary>
        public string Value => _value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public AbbreviationAttribute(string value) : base() => _value = value;
    }
}

using System;

namespace Imagin.Common.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Field)]
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

using System;

namespace Imagin.Common
{
    /// <summary>
    /// A generic alternative for <see cref="System.ComponentModel.DescriptionAttribute"/>, which isn't available in some frameworks.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
    public class DescriptionAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        public DescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}

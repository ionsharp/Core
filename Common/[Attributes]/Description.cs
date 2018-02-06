using System;

namespace Imagin.Common
{
    /// <summary>
    /// A generic alternative for <see langword="System.ComponentModel.DescriptionAttribute"/>, which isn't available in some frameworks.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
    public class DescriptionAttribute : Attribute
    {
        readonly string description;
        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get => description;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Description"></param>
        public DescriptionAttribute(string Description) : base()
        {
            description = Description;
        }
    }
}

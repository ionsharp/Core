using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
    public class DisplayNameAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string DisplayName
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public DisplayNameAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}

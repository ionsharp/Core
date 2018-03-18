using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
    public class DisplayNameAttribute : Attribute
    {
        readonly string displayName;
        /// <summary>
        /// 
        /// </summary>
        public string DisplayName
        {
            get => displayName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DisplayName"></param>
        public DisplayNameAttribute(string DisplayName) : base()
        {
            displayName = DisplayName;
        }
    }
}

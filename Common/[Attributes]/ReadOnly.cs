using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ReadOnlyAttribute : Attribute
    {
        readonly bool isReadOnly;
        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly
        {
            get => isReadOnly;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IsReadOnly"></param>
        public ReadOnlyAttribute(bool IsReadOnly = true) : base()
        {
            isReadOnly = IsReadOnly;
        }
    }
}

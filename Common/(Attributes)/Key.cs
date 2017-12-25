using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public class KeyAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public object Key
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public KeyAttribute(object key)
        {
            Key = key;
        }
    }
}

using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public class KeyAttribute : Attribute
    {
        readonly object key;
        /// <summary>
        /// 
        /// </summary>
        public object Key
        {
            get => key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        public KeyAttribute(object Key) : base()
        {
            key = Key;
        }
    }
}

using Imagin.Common.Data;
using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class LongFormatAttribute : Attribute
    {
        readonly LongFormat format;
        /// <summary>
        /// 
        /// </summary>
        public LongFormat Format
        {
            get
            {
                return format;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kind"></param>
        public LongFormatAttribute(LongFormat Format)
        {
            format = Format;
        }
    }
}

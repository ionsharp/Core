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
            get => format;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Format"></param>
        public LongFormatAttribute(LongFormat Format) : base()
        {
            format = Format;
        }
    }
}

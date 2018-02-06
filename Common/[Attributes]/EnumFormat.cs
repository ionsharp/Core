using System;
using Imagin.Common.Data;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class EnumFormatAttribute : Attribute
    {
        readonly EnumFormat format;
        /// <summary>
        /// 
        /// </summary>
        public EnumFormat Format
        {
            get => format;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Format"></param>
        public EnumFormatAttribute(EnumFormat Format) : base()
        {
            format = Format;
        }
    }
}

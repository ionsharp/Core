using Imagin.Common.Data;
using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class DateFormatAttribute : Attribute
    {
        readonly DateFormat format;
        /// <summary>
        /// 
        /// </summary>
        public DateFormat Format
        {
            get => format;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Format"></param>
        public DateFormatAttribute(DateFormat Format) : base()
        {
            format = Format;
        }
    }
}

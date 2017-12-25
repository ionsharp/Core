using System;
using Imagin.Common.Data;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class StringFormatAttribute : Attribute
    {
        readonly object format;
        /// <summary>
        /// 
        /// </summary>
        public object Format
        {
            get
            {
                return format;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Format"></param>
        StringFormatAttribute(object Format)
        {
            format = Format;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Format"></param>
        public StringFormatAttribute(string Format) : this(Format as object)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Format"></param>
        public StringFormatAttribute(StringFormat Format) : this(Format as object)
        {
        }
    }
}

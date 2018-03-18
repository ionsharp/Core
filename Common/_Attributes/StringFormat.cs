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
            get => format;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Format"></param>
        StringFormatAttribute(object Format) : base()
        {
            format = Format;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Format"></param>
        public StringFormatAttribute(string Format) : this((object)Format)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Format"></param>
        public StringFormatAttribute(StringFormat Format) : this((object)Format)
        {
        }
    }
}

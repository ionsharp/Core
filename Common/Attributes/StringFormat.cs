using Imagin.Common.Primitives;
using System;

namespace Imagin.Common.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class StringFormatAttribute : Attribute
    {
        readonly string format;
        /// <summary>
        /// 
        /// </summary>
        public string Format
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
        public StringFormatAttribute(string Format)
        {
            format = Format;
        }
    }
}

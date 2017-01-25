using Imagin.Common.Primitives;
using System;

namespace Imagin.Common.Attributes
{
    /// <summary>
    /// Specifies how an Int64 field should be displayed.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class Int64KindAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public Int64Kind Kind
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kind"></param>
        public Int64KindAttribute(Int64Kind kind)
        {
            Kind = kind;
        }
    }
}

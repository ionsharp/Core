using Imagin.Common.Primitives;
using System;

namespace Imagin.Common.Attributes
{
    /// <summary>
    /// Specifies how a string field should be displayed.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class StringKindAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public StringKind Kind
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kind"></param>
        public StringKindAttribute(StringKind kind)
        {
            Kind = kind;
        }
    }
}

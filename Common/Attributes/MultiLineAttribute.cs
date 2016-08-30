using System;

namespace Imagin.Common.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class MultiLineAttribute : Attribute
    {
        public bool IsMultiLine
        {
            get; set;
        }

        public MultiLineAttribute(bool IsMultiLine)
        {
            this.IsMultiLine = IsMultiLine;
        }
    }
}

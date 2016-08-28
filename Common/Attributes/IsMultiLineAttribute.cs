using System;

namespace Imagin.Common.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class MultiLine : Attribute
    {
        public bool IsMultiLine
        {
            get; set;
        }

        public MultiLine(bool IsMultiLine)
        {
            this.IsMultiLine = IsMultiLine;
        }
    }
}

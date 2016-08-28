using System;

namespace Imagin.Common.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class Primary : Attribute
    {
        public bool IsPrimary
        {
            get; set;
        }

        public Primary(bool IsPrimary)
        {
            this.IsPrimary = IsPrimary;
        }
    }
}

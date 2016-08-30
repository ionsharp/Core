using System;

namespace Imagin.Common.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class PrimaryAttribute : Attribute
    {
        public bool IsPrimary
        {
            get; set;
        }

        public PrimaryAttribute(bool IsPrimary)
        {
            this.IsPrimary = IsPrimary;
        }
    }
}

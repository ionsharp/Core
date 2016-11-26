using System;

namespace Imagin.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    [Serializable]
    public class FeaturedAttribute : Attribute
    {
        public bool IsFeatured
        {
            get; set;
        }

        public FeaturedAttribute(bool IsFeatured)
        {
            this.IsFeatured = IsFeatured;
        }
    }
}

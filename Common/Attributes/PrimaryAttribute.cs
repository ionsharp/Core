using System;

namespace Imagin.Common.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
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

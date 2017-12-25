using System;

namespace Imagin.Common
{
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

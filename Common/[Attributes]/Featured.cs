using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FeaturedAttribute : Attribute
    {
        readonly bool isFeatured;
        /// <summary>
        /// 
        /// </summary>
        public bool IsFeatured
        {
            get => isFeatured;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IsFeatured"></param>
        public FeaturedAttribute(bool IsFeatured) : base()
        {
            isFeatured = IsFeatured;
        }
    }
}

using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class CategoryAttribute : Attribute
    {
        readonly string category;
        /// <summary>
        /// 
        /// </summary>
        public string Category
        {
            get => category;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Category"></param>
        public CategoryAttribute(string Category) : base()
        {
            category = Category;
        }
    }
}

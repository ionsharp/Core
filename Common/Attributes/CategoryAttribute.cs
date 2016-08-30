using System;

namespace Imagin.Common.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class CategoryAttribute : Attribute
    {
        public string Name
        {
            get; set;
        }

        public CategoryAttribute(string Category)
        {
            this.Name = Category;
        }
    }
}

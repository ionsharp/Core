using System;

namespace Imagin.Common.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class Category : Attribute
    {
        public string Name
        {
            get; set;
        }

        public Category(string Category)
        {
            this.Name = Category;
        }
    }
}

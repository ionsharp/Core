using System;

namespace Imagin.Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SourceAttribute : Attribute
    {
        public readonly string ItemSource;

        public readonly string ItemPath;

        public SourceAttribute(string itemSource, string itemPath = null) : base()
        {
            ItemSource 
                = itemSource;
            ItemPath 
                = itemPath;
        }
    }

}
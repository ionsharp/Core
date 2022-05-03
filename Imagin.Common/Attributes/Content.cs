using System;

namespace Imagin.Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ContentAttribute : Attribute
    {
        public readonly object Content;

        public ContentAttribute(object content) : base() => Content = content;
    }
}
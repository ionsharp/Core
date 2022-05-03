using System;

namespace Imagin.Common
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ContentTriggerAttribute : Attribute
    {
        public readonly string Format;

        public readonly string PropertyName;

        public ContentTriggerAttribute(string propertyName, string format) : base()
        {
            PropertyName
                = propertyName;
            Format
                = format;
        }
    }
}
using System;

namespace Imagin.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class StringFormatAttribute : Attribute
    {
        public readonly string Format;

        public StringFormatAttribute(string format)
            : base() => Format = format;
    }
}
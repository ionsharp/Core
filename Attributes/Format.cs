using System;

namespace Imagin.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FormatAttribute : Attribute
    {
        public readonly object Format;

        public FormatAttribute(object Format) : base() => this.Format = Format;
    }
}
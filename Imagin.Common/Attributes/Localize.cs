using System;

namespace Imagin.Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class LocalizeAttribute : Attribute
    {
        public readonly bool Localize;

        public LocalizeAttribute(bool localize = true) : base() => Localize = localize;
    }
}
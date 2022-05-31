using System;

namespace Imagin.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ReadOnlyAttribute : Attribute
    {
        public readonly bool ReadOnly;

        public ReadOnlyAttribute() : this(true) { }

        public ReadOnlyAttribute(bool readOnly) : base() => ReadOnly = readOnly;
    }
}
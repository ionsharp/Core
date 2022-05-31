using System;

namespace Imagin.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
    public class HiddenAttribute : Attribute
    {
        public readonly bool Hidden;

        public HiddenAttribute() : this(true) { }

        public HiddenAttribute(bool hidden) : base() => Hidden = hidden;
    }
}
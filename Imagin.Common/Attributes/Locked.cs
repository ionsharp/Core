using System;

namespace Imagin.Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class LockedAttribute : Attribute
    {
        public readonly bool Locked;

        public LockedAttribute() : this(true) { }

        public LockedAttribute(bool locked) : base() => Locked = locked;
    }
}
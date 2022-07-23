using System;

namespace Imagin.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class LockAttribute : Attribute
    {
        public readonly bool Locked;

        public LockAttribute() : this(true) { }

        public LockAttribute(bool locked) : base() => Locked = locked;
    }
}
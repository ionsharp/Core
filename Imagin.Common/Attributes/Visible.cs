using System;

namespace Imagin.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
    public class VisibleAttribute : Attribute
    {
        public readonly bool Visible;

        public VisibleAttribute() : this(true) { }

        public VisibleAttribute(bool visible) : base() => Visible = visible;
    }
}
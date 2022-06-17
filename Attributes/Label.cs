using System;

namespace Imagin.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class LabelAttribute : Attribute
    {
        public readonly bool Label;

        public LabelAttribute() : this(true) { }

        public LabelAttribute(bool label) : base() => Label = label;
    }
}
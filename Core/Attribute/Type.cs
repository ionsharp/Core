using System;

namespace Imagin.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
    public class TypeAttribute : Attribute
    {
        public readonly object Type;

        public TypeAttribute(object type) : base() => Type = type;
    }
}
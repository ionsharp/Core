using System;

namespace Imagin.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ValidateAttribute : Attribute
    {
        public readonly Type Type;

        public ValidateAttribute(Type type) : base() => Type = type;
    }
}
using System;

namespace Imagin.Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class AssignableAttribute : Attribute
    {
        public readonly Type[] Types = null;

        public AssignableAttribute(params Type[] types) : base() => Types = types;
    }
}
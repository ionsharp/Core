using System;

namespace Imagin.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class AssignableAttribute : Attribute
    {
        public readonly string Values = null;

        public readonly Type[] Types = null;

        public AssignableAttribute(params Type[] types) : base() => Types = types;

        public AssignableAttribute(string values) : base() => Values = values;
    }
}
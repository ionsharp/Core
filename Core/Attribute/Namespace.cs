using System;

namespace Imagin.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
    public class NamespaceAttribute : Attribute
    {
        public readonly string Value = null;

        public NamespaceAttribute(params string[] @namespace) : base()
        {
            foreach (var i in @namespace)
            {
                if (Value == null)
                {
                    Value = i;
                    continue;
                }
                Value = $"{Value}.{i}";
            }
        }
    }
}
using System;

namespace Imagin.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property)]
    public class IndexAttribute : Attribute
    {
        public readonly int Index;

        public IndexAttribute(int index = -1) : base() => Index = index;
    }
}
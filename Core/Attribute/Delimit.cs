using System;

namespace Imagin.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DelimitAttribute : Attribute
    {
        public readonly char Character;

        public DelimitAttribute(char i) : base() => Character = i;
    }
}
using System;

namespace Imagin.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class OptionAttribute : Attribute
    {
        public OptionAttribute() { }
    }
}
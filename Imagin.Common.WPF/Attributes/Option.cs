using System;

namespace Imagin.Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class OptionAttribute : Attribute
    {
        public OptionAttribute() { }
    }
}
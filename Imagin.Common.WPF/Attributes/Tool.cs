using System;

namespace Imagin.Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ToolAttribute : Attribute
    {
        public ToolAttribute() { }
    }
}
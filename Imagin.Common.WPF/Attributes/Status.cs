using System;

namespace Imagin.Common.Controls
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class StatusAttribute : Attribute
    {
        public StatusAttribute() { }
    }
}
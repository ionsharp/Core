using System;

namespace Imagin.Common.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class HideAttribute : Attribute
    {
        public bool Value
        {
            get; set;
        }

        public HideAttribute(bool Value)
        {
            this.Value = Value;
        }
    }
}

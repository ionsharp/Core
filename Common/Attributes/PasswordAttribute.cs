using System;

namespace Imagin.Common.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class PasswordAttribute : Attribute
    {
        public bool Value
        {
            get; set;
        }

        public PasswordAttribute(bool Value)
        {
            this.Value = Value;
        }
    }
}

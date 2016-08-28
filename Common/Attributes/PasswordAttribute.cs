using System;

namespace Imagin.Common.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class Password : Attribute
    {
        public bool Value
        {
            get; set;
        }

        public Password(bool Value)
        {
            this.Value = Value;
        }
    }
}

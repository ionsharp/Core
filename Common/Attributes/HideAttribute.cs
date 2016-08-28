using System;

namespace Imagin.Common.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class Hide : Attribute
    {
        public bool Value
        {
            get; set;
        }

        public Hide(bool Value)
        {
            this.Value = Value;
        }
    }
}

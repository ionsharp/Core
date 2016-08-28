using System;

namespace Imagin.Common.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class File : Attribute
    {
        public bool Value
        {
            get; set;
        }

        public File(bool Value)
        {
            this.Value = Value;
        }
    }
}

using System;

namespace Imagin.Common.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class FileAttribute : Attribute
    {
        public bool Value
        {
            get; set;
        }

        public FileAttribute(bool Value)
        {
            this.Value = Value;
        }
    }
}

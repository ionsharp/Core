using System;

namespace Imagin.Common.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Field)]
    public class AbbreviationAttribute : Attribute
    {
        public string Value
        {
            get; set;
        }

        public AbbreviationAttribute(string Value)
        {
            this.Value = Value;
        }
    }
}

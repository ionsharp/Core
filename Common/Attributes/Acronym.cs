using System;

namespace Imagin.Common.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Field)]
    public class AcronymAttribute : Attribute
    {
        public string Value
        {
            get; set;
        }

        public AcronymAttribute(string Value)
        {
            this.Value = Value;
        }
    }
}

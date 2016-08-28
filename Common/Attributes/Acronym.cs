using System;

namespace Imagin.Common.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Field)]
    public class Acronym : Attribute
    {
        public string Value
        {
            get; set;
        }

        public Acronym(string Value)
        {
            this.Value = Value;
        }
    }
}

using System;

namespace Imagin.Common.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class AliasAttribute : Attribute
    {
        public object Alias
        {
            get; set;
        }

        public AliasAttribute(object Alias)
        {
            this.Alias = Alias;
        }
    }
}

using System;

namespace Imagin.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    [Serializable]
    public class IconAttribute : Attribute
    {
        public string Uri
        {
            get; private set;
        }

        public IconAttribute(string Uri)
        {
            this.Uri = Uri;
        }
    }
}

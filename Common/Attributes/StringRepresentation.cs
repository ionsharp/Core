using Imagin.Common.Text;
using System;

namespace Imagin.Common.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class StringRepresentationAttribute : Attribute
    {
        public StringRepresentation Representation
        {
            get; private set;
        }

        public StringRepresentationAttribute(StringRepresentation Representation)
        {
            this.Representation = Representation;
        }
    }
}

using Imagin.Common.Text;
using System;

namespace Imagin.Common.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class Int64RepresentationAttribute : Attribute
    {
        public Int64Representation Representation
        {
            get; private set;
        }

        public Int64RepresentationAttribute(Int64Representation Representation)
        {
            this.Representation = Representation;
        }
    }
}

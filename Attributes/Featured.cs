using System;

namespace Imagin.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FeaturedAttribute : Attribute
    {
        public readonly bool Featured;

        public readonly AboveBelow Where;

        public FeaturedAttribute() : this(true) { }

        public FeaturedAttribute(AboveBelow where) : this(true, where) { }

        public FeaturedAttribute(bool featured, AboveBelow where = AboveBelow.Above) : base()
        {
            Featured = featured;
            Where = where;
        }
    }
}
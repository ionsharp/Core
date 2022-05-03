using System.Reflection;

namespace Imagin.Common.Controls
{
    public class MemberData
    {
        public readonly MemberAttributes Attributes;

        public readonly MemberCollection Collection;

        public readonly MemberInfo Member;

        public readonly MemberSource Source;

        protected MemberData(MemberData data) : this(data.Collection, data.Source, data.Member, data.Attributes) { }

        public MemberData(MemberCollection collection, MemberSource source, MemberInfo member, MemberAttributes attributes)
        {
            Collection 
                = collection;
            Source
                = source;
            Member 
                = member;
            Attributes
                = attributes;
        }
    }
}
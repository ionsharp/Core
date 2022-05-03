using System;

namespace Imagin.Common.Controls
{
    #region (public) MemberRouteElement

    public abstract class MemberRouteElement
    {
        public virtual string Name { get; }

        public virtual object Value { get; }

        public Array Values => IsArray ? (Array)Value : Array<object>.New(Value);

        public bool IsArray => Value.GetType().IsArray;

        public MemberRouteElement() { }
    }

    #endregion

    #region (public) MemberRouteData

    public class MemberRouteData
    {
        public readonly MemberRouteElement Element;

        public MemberRouteData(MemberRouteElement i) => Element = i;
    }

    #endregion

    #region (public) MemberRouteChild

    public class MemberRouteChild : MemberRouteElement
    {
        public override string Name => Member.Name;

        public override object Value => Member.Value;

        public MemberModel Member { get; private set; }

        public MemberRouteChild(MemberModel member) : base() => Member = member;
    }

    #endregion

    #region (public) MemberRouteItem

    public class MemberRouteItem : MemberRouteChild
    {
        public override string Name => $"{(Member as ListItemModel).Parent.Name}[]";

        public MemberRouteItem(MemberModel member) : base(member) { }
    }

    #endregion

    #region (public) MemberRouteSource

    public class MemberRouteSource : MemberRouteElement
    {
        public Type Type => Value?.GetType();

        readonly object value;
        public override object Value => value;

        public MemberRouteSource(object i) : base() => value = i;
    }

    #endregion
}
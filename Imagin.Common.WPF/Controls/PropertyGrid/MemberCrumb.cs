namespace Imagin.Common.Controls
{
    public abstract class MemberCrumb : Base
    {
        MemberRouteElement element = null;
        public MemberRouteElement Element
        {
            get => element;
            private set => this.Change(ref element, value);
        }

        public MemberCrumb(MemberRouteElement element) : base() => Element = element;
    }

    public class MemberCrumbChild : MemberCrumb
    {
        public MemberCrumbChild(MemberRouteChild input) : base(input) { }
    }

    public class MemberCrumbItem : MemberCrumb
    {
        public MemberCrumbItem(MemberRouteItem input) : base(input) { }
    }

    public class MemberCrumbSource : MemberCrumb
    {
        public MemberCrumbSource(MemberRouteSource input) : base(input) { }
    }
}
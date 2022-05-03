using Imagin.Common.Data;
using Imagin.Common.Linq;
using System;
using System.Reflection;

namespace Imagin.Common.Controls
{
    public class ListItemModel : MemberModel
    {
        public readonly ListMemberModel Parent;

        public int ParentIndex => Parent.Items.IndexOf(this);

        //...

        public override bool CanWrite => !Parent.IsReadOnly;

        public override bool IsReadOnly => !CanWrite;

        public override MemberInfo Member => Parent.Member;

        public override object Format => Parent.Attributes.Get<ItemFormatAttribute>()?.Format;

        public override object Style => Parent.Attributes.Get<ItemStyleAttribute>()?.Style;

        public override Type Type => Parent?.ItemType;

        //...

        public ListItemModel(ListMemberModel parent, MemberData data) : base(data)
        {
            Parent = parent;
        }

        //...

        protected override void Apply(MemberAttributes input) { }

        protected override void SetValue(object source, object value) 
            => ParentIndex.If(i => i > -1 && Parent.ActualValue?.Count > i, i => Parent.ActualValue[i] = value);

        protected override object GetValue(object input)
        {
            object result = null;
            ParentIndex.If(i => i > -1 && Parent.ActualValue?.Count > i, i => result = Parent.ActualValue[i]);
            return result;
        }
    }
}
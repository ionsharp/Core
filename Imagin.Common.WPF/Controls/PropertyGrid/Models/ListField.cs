using System;
using System.Reflection;

namespace Imagin.Common.Controls
{
    public class ListFieldModel : ListMemberModel
    {
        public override bool CanWrite => true;

        new public FieldInfo Member => (FieldInfo)base.Member;

        public override Type Type => Member.FieldType;

        //...

        public ListFieldModel(MemberData data) : base(data) { }

        //...

        protected override object GetValue(object input) => Member.GetValue(input);

        protected override void SetValue(object source, object value) => Member.SetValue(source, value);
    }
}
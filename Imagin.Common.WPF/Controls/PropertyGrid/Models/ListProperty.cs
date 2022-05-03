using System;
using System.Reflection;

namespace Imagin.Common.Controls
{
    public class ListPropertyModel : ListMemberModel
    {
        public override bool CanWrite => Member.GetSetMethod(true) != null;

        new public PropertyInfo Member => (PropertyInfo)base.Member;

        public override Type Type => Member.PropertyType;

        //...

        public ListPropertyModel(MemberData data) : base(data) { }

        //...

        protected override void SetValue(object source, object value) => Member.SetValue(source, value, null);

        protected override object GetValue(object input) => Member.GetValue(input);
    }
}